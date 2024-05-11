using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Helpers;
using Backend.Dtos;
namespace Backend.Services
{
    public class UserService
    {
        private readonly EcommerceSdaContext _dbContext;

        // Constructor for initializing the UserService with the provided database context.
        public UserService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        // Converts fetched database data (users) into DTOs by selecting/mapping specific properties and creating new objects.
        public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
        {
            try
            {
                // fetches all users from the database
                var users = await _dbContext.Users.ToListAsync();
                // Map fetched database entities to DTOs by selecting specific properties
                var userDtos = users.Select(u => new GetUserDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Mobile = u.Mobile,
                    IsAdmin = u.IsAdmin,
                    IsBanned = u.IsBanned
                }).ToList();
                return userDtos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving Users.", ex);
            }
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _dbContext.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving user with ID {userId}.", ex);
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                // Generate a unique identifier for the user using IdGenerator helper.
                user.UserId = await IdGenerator.GenerateIdAsync<User>(_dbContext);
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while creating user.", ex);
            }
        }

        public async Task<User?> UpdateUserAsync(int userId, User user)
        {
            try
            {
                var existingUser = await _dbContext.Users.FindAsync(userId);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
                    existingUser.LastName = user.LastName ?? existingUser.LastName;
                    existingUser.Email = user.Email ?? existingUser.Email;
                    existingUser.Mobile = user.Mobile ?? existingUser.Mobile;
                    existingUser.Password = user.Password ?? existingUser.Password;
                    existingUser.IsAdmin = user.IsAdmin;
                    existingUser.IsBanned = user.IsBanned;
                    await _dbContext.SaveChangesAsync();
                    return existingUser;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while updating user with ID {userId}.", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var userToDelete = await _dbContext.Users.FindAsync(userId);
                if (userToDelete != null)
                {
                    _dbContext.Users.Remove(userToDelete);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while deleting user with ID {userId}.", ex);
            }
        }
    }
}