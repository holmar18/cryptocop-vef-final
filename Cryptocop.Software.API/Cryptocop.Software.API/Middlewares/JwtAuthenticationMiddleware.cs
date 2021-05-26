using System.Linq;
using System.Text;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Cryptocop.Software.API.Middlewares
{
    public static class JwtAuthenticationMiddleware
    {
        public static AuthenticationBuilder AddJwtTokenAuthentication(this AuthenticationBuilder builder, IConfiguration config)
        {
            var jwtConfig = config.GetSection("JwtConfig");
            var secret = jwtConfig.GetSection("secret").Value;
            var issuer = jwtConfig.GetSection("issuer").Value;
            var audience = jwtConfig.GetSection("audience").Value;
            var key = Encoding.ASCII.GetBytes(secret);
            // var expirationInMinutes = jwtConfig.GetValue<string>("expirationInMinutes");

            builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x => 
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    NameClaimType = "name" // User.Identity.Name
                };
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var claim = context.Principal.Claims.FirstOrDefault(r => r.Type == "tokenId").Value;
                        int.TryParse(claim, out var tokenId);
                        var accountService = context.HttpContext.RequestServices.GetService<ITokenRepository>();

                        if (accountService.IsTokenBlacklisted(tokenId))
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Invalid Jwt token");
                        }
                    }
                };
            });
        return builder;
        }
    }
}