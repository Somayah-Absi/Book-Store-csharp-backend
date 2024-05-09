using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Backend.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace auth.Helpers
{
    public class JwtService
    {


        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwt(UserDto user)
        {

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            bool isAdmin = user.IsAdmin ?? false;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, isAdmin? "Admin" : "User"),
            }),

                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
