using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Microservices.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(MSUser user)
        {
            if (user is null) return "";

            var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            //claims are pieces of information asserted about a subject.
            //a jwtToken can contain a claim called "name" that asserts that the name of the user authenticating is "John Doe".
            //In a JWT, a claim appears as a name/value pair where the name is always a string and the value can be any JSON value.
            //in the following case, the user "claims" that those are its name, id and email
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Name!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
