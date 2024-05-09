using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;

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
                var users = await _userService.GetAllUsersAsync();
                return ApiResponse.Success(users, "all users are returned successfully");
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
                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, User user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, user);
                if (updatedUser == null)
                {
                    return ApiResponse.NotFound("User was not found");

                }
                else
                {
                    return ApiResponse.Success(updatedUser, "Update user successfully");
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
            catch (Exception ex)
            {
                return ApiResponse.ServerError(ex.Message);
            }
        }
    }
}