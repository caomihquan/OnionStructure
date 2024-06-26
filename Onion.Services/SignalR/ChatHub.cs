﻿using Microsoft.AspNetCore.SignalR;
using Onion.Datas;
using Onion.Datas.Abstract;
using Onion.Domains.Entities;
using Onion.Domains.Extension;
using Onion.Domains.Models;
using Onion.Services.RoomServices;
using Onion.Services.SignalR;
using Onion.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Onion.Domains.SignalR
{
    public class ChatHub : Hub
    {
        //IMapper _mapper;
        IHubContext<PresenceHub> _presenceHub;
        PresenceTracker _presenceTracker;
        UserShareScreenTracker _shareScreenTracker;
        IUserService _userService;
        IRoomServices _roomService;
        IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork,UserShareScreenTracker shareScreenTracker, PresenceTracker presenceTracker, IHubContext<PresenceHub> presenceHub, IUserService userService,IRoomServices roomServices)
        {
            //_mapper = mapper;
            _unitOfWork = unitOfWork;
            _presenceTracker = presenceTracker;
            _presenceHub = presenceHub;
            _shareScreenTracker = shareScreenTracker;
            _userService = userService;
            _roomService = roomServices;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var roomId = httpContext.Request.Query["roomId"].ToString();
            var roomIdInt = int.Parse(roomId);
            var username = Context.User.GetUsername();

            await _presenceTracker.UserConnected(new UserConnectionInfo(username, roomIdInt), Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);//khi user click vao room se join vao
            await AddConnectionToGroup(roomIdInt); // luu db DbSet<Connection> de khi disconnect biet

            //var usersOnline = await _unitOfWork.UserRepository.GetUsersOnlineAsync(currentUsers);
            var oneUserOnline = _unitOfWork.GetService<IUserService, User>().GetMemberAsync(username);
            //var oneUserOnline = _unitOfWork.GetService<IUserReponsitory, User>().getUseryUserName(username);
            await Clients.Group(roomId).SendAsync("UserOnlineInGroup", oneUserOnline);

            var currentUsers = await _presenceTracker.GetOnlineUsers(roomIdInt);
            await _roomService.UpdateCountMember(roomIdInt, currentUsers.Length);
            //await _unitOfWork.Complete();

            var currentConnections = await _presenceTracker.GetConnectionsForUser(new UserConnectionInfo(username, roomIdInt));
            await _presenceHub.Clients.AllExcept(currentConnections).SendAsync("CountMemberInGroup",
                   new { roomId = roomIdInt, countMember = currentUsers.Length });

            //share screen user vao sau cung
            var userIsSharing = await _shareScreenTracker.GetUserIsSharing(roomIdInt);
            if (userIsSharing != null)
            {
                var currentBeginConnectionsUser = await _presenceTracker.GetConnectionsForUser(userIsSharing);
                if (currentBeginConnectionsUser.Count > 0)
                    await Clients.Clients(currentBeginConnectionsUser).SendAsync("OnShareScreenLastUser", new { usernameTo = username, isShare = true });
                await Clients.Caller.SendAsync("OnUserIsSharing", userIsSharing.UserName);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.GetUsername();
            var group = await RemoveConnectionFromGroup();
            var isOffline = await _presenceTracker.UserDisconnected(new UserConnectionInfo(username, group.RoomId), Context.ConnectionId);

            await _shareScreenTracker.DisconnectedByUser(username, group.RoomId);
            if (isOffline)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group.RoomId.ToString());
                var temp = await _userService.GetMemberAsync(username);
                await Clients.Group(group.RoomId.ToString()).SendAsync("UserOfflineInGroup", temp);

                var currentUsers = await _presenceTracker.GetOnlineUsers(group.RoomId);

                await _roomService.UpdateCountMember(group.RoomId, currentUsers.Length);
                //await _unitOfWork.Complete();

                await _presenceHub.Clients.All.SendAsync("CountMemberInGroup",
                       new { roomId = group.RoomId, countMember = currentUsers.Length });
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var userName = Context.User.GetUsername();
            var sender = await _userService.GetMemberAsync(userName);
            var group = await _roomService.GetRoomForConnection(Context.ConnectionId);

            if (group != null)
            {
                var message = new MessageDto
                {
                    SenderUsername = userName,
                    SenderDisplayName = sender.DisplayName,
                    Content = createMessageDto.Content,
                    MessageSent = DateTime.Now
                };
                //Luu message vao db
                //code here
                //send meaasge to group
                await Clients.Group(group.RoomId.ToString()).SendAsync("NewMessage", message);
            }
        }

        public async Task MuteMicro(bool muteMicro)
        {
            var group = await _roomService.GetRoomForConnection(Context.ConnectionId);
            if (group != null)
            {
                await Clients.Group(group.RoomId.ToString()).SendAsync("OnMuteMicro", new { username = Context.User.GetUsername(), mute = muteMicro });
            }
            else
            {
                throw new HubException("group == null");
            }
        }

        public async Task MuteCamera(bool muteCamera)
        {
            var group = await _roomService.GetRoomForConnection(Context.ConnectionId);
            if (group != null)
            {
                await Clients.Group(group.RoomId.ToString()).SendAsync("OnMuteCamera", new { username = Context.User.GetUsername(), mute = muteCamera });
            }
            else
            {
                throw new HubException("group == null");
            }
        }

        public async Task ShareScreen(int roomid, bool isShareScreen)
        {
            if (isShareScreen)//true is doing share
            {
                await _shareScreenTracker.UserConnectedToShareScreen(new UserConnectionInfo(Context.User.GetUsername(), roomid));
                await Clients.Group(roomid.ToString()).SendAsync("OnUserIsSharing", Context.User.GetUsername());
            }
            else
            {
                await _shareScreenTracker.UserDisconnectedShareScreen(new UserConnectionInfo(Context.User.GetUsername(), roomid));
            }
            await Clients.Group(roomid.ToString()).SendAsync("OnShareScreen", isShareScreen);
            //var group = await _unitOfWork.RoomRepository.GetRoomForConnection(Context.ConnectionId);
        }

        public async Task ShareScreenToUser(int roomid, string username, bool isShare)
        {
            var currentBeginConnectionsUser = await _presenceTracker.GetConnectionsForUser(new UserConnectionInfo(username, roomid));
            if (currentBeginConnectionsUser.Count > 0)
                await Clients.Clients(currentBeginConnectionsUser).SendAsync("OnShareScreen", isShare);
        }

        private async Task<Room> RemoveConnectionFromGroup()
        {
            var group = await _roomService.GetRoomForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _roomService.RemoveConnection(connection);

            //if (await _unitOfWork.Complete()) 
                return group;

            throw new HubException("Fail to remove connection from room");
        }

        private async Task<Room> AddConnectionToGroup(int roomId)
        {
            var group = await _roomService.GetRoomById(roomId);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());
            if (group != null)
            {
                group.Connections.Add(connection);
            }

            //if (await _unitOfWork.Complete()) 
                return group;

            throw new HubException("Failed to add connection to room");
        }
    }
}
