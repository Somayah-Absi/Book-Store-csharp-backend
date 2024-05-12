using auth.Data;
using Backend.Dtos;
using auth.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using auth.Dtos;



namespace auth.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        // Constructor
        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            // Create a new User object using the data from the RegisterDto
            var user = new User
            {
                FirstName = dto.FirstName ?? "", // Set the first name from the DTO (or an empty string if null)
                LastName = dto.LastName ?? "",   // Set the last name from the DTO (or an empty string if null)
                Email = dto.Email ?? "",         // Set the email from the DTO (or an empty string if null)
                                                 // Hash the password using BCrypt.Net and set it in the user object
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Mobile = dto.Mobile,              // Set the mobile number from the DTO
            };

            // Create the user in the repository and return the result with a "Created" status code
            return Created("Successfully Registered", _repository.Create(user));
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                // Retrieve the user from the repository by email
                var user = _repository.GetByEmail(dto.Email ?? "");

                // If no user is found with the provided email, return a BadRequest response
                if (user == null) return BadRequest(new { message = "Invalid Email ❗" });

                // Verify the password using BCrypt.Net
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                {
                    // If the password is invalid, return a BadRequest response
                    return BadRequest(new { message = "Invalid Password ❗" });
                }

                // Create a UserDto object from the retrieved user
                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    IsAdmin = user.IsAdmin,
                    IsBanned = user.IsBanned,
                };

                // Generate a JWT token for the authenticated user
                var jwt = _jwtService.GenerateJwt(userDto);

                // Add the JWT token to a cookie for future authentication
                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    HttpOnly = true
                });

                // Return a successful response with a welcome message
                return Ok(new
                {
                    message = $"Successfully logged in, welcome back again {userDto.FirstName} 🌸 ",
                });
            }
            catch (Exception e)
            {
                // If an exception occurs during login, return a BadRequest response with the error message
                return BadRequest($"{e.Message}");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Check if the "jwt" cookie exists in the request
            if (Request.Cookies.ContainsKey("jwt"))
            {
                // If the cookie exists, delete it from the response
                Response.Cookies.Delete("jwt");

                // Return a successful response indicating successful logout
                return Ok(new { message = "Logged out successfully ✔️" });
            }
            else
            {
                // If the "jwt" cookie does not exist, return a NotFound response
                return NotFound(new { message = "You are not logged in ❗ " });
            }
        }
    }
}