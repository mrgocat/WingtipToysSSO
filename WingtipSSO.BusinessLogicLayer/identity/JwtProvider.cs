using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WingtipSSO.POCOS;

namespace WingtipSSO.BusinessLogicLayer.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        public JwtProvider(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public string GenerateJwtToken(UserPoco user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };
            if (!string.IsNullOrEmpty(user.Nationality)) claims.Add(new Claim("Nationality", user.Nationality));
            if (user.DateOfBirth != null) claims.Add(new Claim("DateOfBirth", ((DateTime)user.DateOfBirth).ToString("yyyy-MM-dd")));
            if (user.IsUser) claims.Add(new Claim(ClaimTypes.Role, "User"));
            if (user.IsAdmin && user.Roles != null && user.Roles.Count > 0)
            {
            //    claims.Add(new Claim(ClaimTypes.Role, string.Join(",", user.Roles)));
                claims.Add(new Claim(ClaimTypes.Role, user.Roles.First().ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtOptions.JwtExpireDays);

            var token = new JwtSecurityToken(
                _jwtOptions.JwtIssuer,
                _jwtOptions.JwtAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
