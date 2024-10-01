using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Services;

namespace TalabatServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configurations;

        public TokenService(IConfiguration configurations)
        {
            _configurations = configurations;
        }

        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.GivenName, user.DisplayName),
                new Claim (ClaimTypes.Email, user.Email),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_configurations["JWT:Key"]));
            var token = new JwtSecurityToken
                (
                issuer: _configurations["JWT:validIssuer"],
                audience: _configurations["JWT:validAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configurations["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
