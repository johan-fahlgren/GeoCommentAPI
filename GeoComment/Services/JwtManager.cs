﻿using GeoComment.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GeoComment.Services
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Källa för kod:
        // https://dev.to/moe23/asp-net-core-5-rest-api-authentication-with-jwt-step-by-step-140d
        public string GenerateJwtToken(GeoUser geoUser)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, geoUser.Id),
                    new Claim(ClaimTypes.Name, geoUser.UserName),
                    // Eventuella roller måste läggas till här
                }),
                // Hur länge token ska vara giltlig
                Expires = DateTime.UtcNow.AddHours(6),
                // Den hemliga nyckeln och vilken typ av kryptering som används
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtEncryptedToken = jwtTokenHandler.WriteToken(token);
            return jwtEncryptedToken;
        }
    }
}
