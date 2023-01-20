using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gemniczak.API.Config
{
    public static class ConfigureAuthentication
    {
        public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value)),
                        ValidateIssuer = true,
                        ValidIssuer = config.GetSection("Jwt:Issuer").Value,
                        ValidateAudience = true,
                        ValidAudience = config.GetSection("Jwt:Audiance").Value
                    };
                });

            services.AddCors(option => option.AddPolicy(name: "NgOrigins",
                policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyHeader();
                }));
        }
    }
}
