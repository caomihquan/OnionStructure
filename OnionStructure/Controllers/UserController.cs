using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onion.Domains.Entities;
using OnionStructure.ViewModel;

namespace OnionStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UserModel userModel = new()
            {
                Id = 10,
                UserName = "cmquan",
                Password = "khongbiet",
                FullName = "CMQUAN"
            };

            User user = _mapper.Map<User>(userModel);

            return Ok(user);
        }
    }
}
