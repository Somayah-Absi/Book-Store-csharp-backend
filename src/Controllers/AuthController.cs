using auth.Data;
using Backend.Dtos;
using auth.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using auth.Dtos;
using Microsoft.AspNetCore.Authorization;



namespace auth.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName ?? "",
                LastName = dto.LastName ?? "",
                Email = dto.Email ?? "",
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Mobile = dto.Mobile,
            };

            return Created("Successfully Registered ✔️", _repository.Create(user));
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = _repository.GetByEmail(dto.Email ?? "");

                if (user == null) return BadRequest(new { message = "Invalid Email ❗" });

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                {
                    return BadRequest(new { message = "Invalid Password ❗" });
                }

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
                var jwt = _jwtService.GenerateJwt(userDto);

                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    HttpOnly = true
                });

                return Ok(new
                {
                    message = $"Successfully logged in, welcome back again {userDto.FirstName} 🌸 ",
                });
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("jwt"))
            {
                Response.Cookies.Delete("jwt");

                return Ok(new { message = "Logged out successfully ✔️" });
            }
            else
            {
                return NotFound(new { message = "You are not logged in ❗ " });
            }
        }
    }
}