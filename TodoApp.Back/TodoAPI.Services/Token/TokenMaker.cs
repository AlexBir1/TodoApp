

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Token
{
    public static class TokenMaker
    {
        public static TokenModel CreateToken(Account model, TokenDescriptorModel descriptor)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.MobilePhone, model.PhoneNumber),
            };

            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(descriptor.Key));
            var credetials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(descriptor.ExpiresInDays),
                SigningCredentials = credetials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var JWT = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenModel
            {
                Token = tokenHandler.WriteToken(JWT),
                ValidTo = DateTime.Now.AddDays(descriptor.ExpiresInDays)
            };
        }
    }
}
