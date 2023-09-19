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
        //IOptions is used here as JwtOptions is placed in the DI as a token configuration, not a service
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(MSUser user, IEnumerable<string> roles)
        {
            if (user == null) return "";

            var claims = CreateClaims(user, roles);

            var tokenDescriptor = GenerateTokenDescriptor(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //the token can be decoded back into an object at https://jwt.io/
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        private List<Claim> CreateClaims(MSUser user, IEnumerable<string> roles)
        {
            //in the following case, the user "claims" that those are its name, id and email
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Name!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            //adds a freshly instantiated Claim object to the list with the role from the parameter passed in (not recommended for security reasons)
            //DECIDING WHICH APPROACH TO FOLLOW HERE
            claims.AddRange(
                roles.Select(role =>
                    new Claim(ClaimTypes.Role, role)
            ));

            return claims;
        }

        private SecurityTokenDescriptor GenerateTokenDescriptor(List<Claim> claims)
        {

            var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            return new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
