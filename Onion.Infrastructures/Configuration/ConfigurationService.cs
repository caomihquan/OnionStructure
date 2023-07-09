using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Onion.Authentication.Service;
using Onion.Cache.Cache;
using Onion.Datas;
using Onion.Datas.Abstract;
using Onion.Services.CategoryService;
using Onion.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastructures.Configuration
{
    public static class ConfigurationService
    {
        public static void RegisterDbContext(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddDbContext<OnionDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("OnionConnection"),
            options=> options.MigrationsAssembly(typeof(OnionDbContext).Assembly.FullName)));
        }

        public static void RegisterDI(this IServiceCollection service)
        {
            service.AddScoped(typeof(IResponsitory<>), typeof(Responsitory<>));
            service.AddScoped<IDapperHelper,DapperHelper>();
            service.AddScoped<ICategoryService,CategoryService>();
            service.AddScoped<IUserService,UserService>();
            service.AddScoped<ITokenHandler,TokenHandler>();
            service.AddSingleton<IDistributedCacheService, DistributedCacheService>();
        }
    }
}
