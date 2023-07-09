using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Onion.Authentication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Onion.Infrastructures.Configuration
{
    public static class ConfigToken
    {
        public static void AddTokenBear(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidIssuer = configuration["JWTToken:Issuer"],
                            ValidateIssuer = false,
                            ValidAudience = configuration["JWTToken:Audience"],
                            ValidateAudience = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTToken:SignatureKey"])),
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                        {
                            OnMessageReceived = context =>
                            {
                                return Task.CompletedTask;

                            },
                            OnTokenValidated = context =>
                            {
                                var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<ITokenHandler>();
                                return tokenHandler.ValidateToken(context);
                            },
                            OnAuthenticationFailed = context =>
                            {
                                return Task.CompletedTask;
                            },
                            OnChallenge = context =>
                            {
                                return Task.CompletedTask;
                            },
                        };
                    });
        }
    }
}
