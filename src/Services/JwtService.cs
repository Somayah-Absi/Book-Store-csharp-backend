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
            // Retrieve the JWT key from configuration
            var jwtKey = _configuration["Jwt:Key"];

            // Check if the key is null or empty
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured properly.");
            }

            // Convert the key to bytes
            var key = Encoding.ASCII.GetBytes(jwtKey);
            // Check if the key is null
            if (key == null || key.Length == 0)
            {
                throw new InvalidOperationException("JWT key is not configured properly.");
            }
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
