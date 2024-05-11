using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Backend.Dtos;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt"))
                {
                    return Unauthorized("Not have any token to access");
                }
                else
                {
                    var jwt = Request.Cookies["jwt"];
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == "Admin");

                    bool isAdmin = isAdminClaim != null;

                    if (isAdmin)
                    {
                        //"Admin access granted"
                        var users = await _userService.GetAllUsersAsync();
                        return ApiResponse.Success(users, "all users are returned successfully");
                    }
                    else
                    {
                        return Unauthorized("You don't have permission to access this endpoint");
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt"))
                {
                    return Unauthorized("Not have any token to access");
                }
                else
                {
                    var jwt = Request.Cookies["jwt"];
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == "Admin");

                    bool isAdmin = isAdminClaim != null;

                    if (isAdmin)
                    {
                        //"Admin access granted"
                        var user = await _userService.GetUserByIdAsync(userId);
                        if (user != null)
                        {
                            return ApiResponse.Created(user);
                        }
                        else
                        {
                            return ApiResponse.NotFound("User was not found");
                        }
                    }
                    else
                    {
                        return Unauthorized("You don't have permission to access this endpoint");
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt"))
                {
                    return Unauthorized("Not have any token to access");
                }
                else
                {
                    var jwt = Request.Cookies["jwt"];
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == "Admin");

                    bool isAdmin = isAdminClaim != null;

                    if (isAdmin)
                    {
                        //"Admin access granted"
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        user.Password = hashedPassword;
                        var createdUser = await _userService.CreateUserAsync(user);
                        var test = CreatedAtAction(nameof(GetUser), new { userId = createdUser.UserId }, createdUser);
                        if (test == null)
                        {
                            return ApiResponse.NotFound("Failed to create a user");
                        }
                        return ApiResponse.Created(user, "User created successfully");
                    }
                    else
                    {
                        return Unauthorized("You don't have permission to access this endpoint");
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt"))
                {
                    return Unauthorized("Not have any token to access");
                }
                else
                {
                    var jwt = Request.Cookies["jwt"];
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == "Admin");

                    bool isAdmin = isAdminClaim != null;

                    if (isAdmin)
                    {
                        //"Admin access granted"
                        // Fetch the existing user from the database
                        var existingUser = await _userService.GetUserByIdAsync(userId);
                        if (existingUser == null)
                        {
                            return ApiResponse.NotFound("User was not found");
                        }
                        // Update only the properties that are provided in the DTO
                        if (updateUserDto.FirstName != null)
                        {
                            existingUser.FirstName = updateUserDto.FirstName;
                        }

                        if (updateUserDto.LastName != null)
                        {
                            existingUser.LastName = updateUserDto.LastName;
                        }

                        if (updateUserDto.Email != null)
                        {
                            existingUser.Email = updateUserDto.Email;
                        }

                        if (updateUserDto.Mobile != null)
                        {
                            existingUser.Mobile = updateUserDto.Mobile;
                        }

                        if (updateUserDto.Password != null)
                        {
                            // Hash the new password before updating
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
                            existingUser.Password = hashedPassword;
                        }

                        if (updateUserDto.IsAdmin != null)
                        {
                            existingUser.IsAdmin = updateUserDto.IsAdmin;
                        }

                        if (updateUserDto.IsBanned != null)
                        {
                            existingUser.IsBanned = updateUserDto.IsBanned;
                        }
                        //"Admin access granted"
                        var updatedUser = await _userService.UpdateUserAsync(userId, existingUser);
                        if (updatedUser == null)
                        {
                            return ApiResponse.NotFound("User was not found");

                        }
                        else
                        {
                            return ApiResponse.Success(updatedUser, "Update user successfully");
                        }
                    }
                    else
                    {
                        return Unauthorized("You don't have permission to access this endpoint");
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                if (!Request.Cookies.ContainsKey("jwt"))
                {
                    return Unauthorized("Not have any token to access");
                }
                else
                {
                    var jwt = Request.Cookies["jwt"];
                    // Validate and decode JWT token to extract claims
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);

                    var isAdminClaim = token.Claims.FirstOrDefault(c => c.Type == "role" && c.Value == "Admin");

                    bool isAdmin = isAdminClaim != null;

                    if (isAdmin)
                    {
                        //"Admin access granted"
                        var result = await _userService.DeleteUserAsync(userId);
                        if (!result)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return ApiResponse.NotFound("User was not found");
                        }
                    }
                    else
                    {
                        return Unauthorized("You don't have permission to access this endpoint");
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }
    }
}