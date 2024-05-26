using Backend.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<PaginationResult<GetUserDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 100)
        {
            try
            {
                // Query to get total count of users
                var totalCountQuery = _dbContext.Users.AsQueryable();
                var totalCount = await totalCountQuery.CountAsync();

                // Query to filter out admin users and get count of non-admin users
                var nonAdminUsersQuery = totalCountQuery.Where(u => u.IsAdmin == false || u.IsAdmin == null);
                var nonAdminCount = await nonAdminUsersQuery.CountAsync();

                // Subtract the count of admin users from the total count of users
                var totalCountExcludingAdmin = totalCount - nonAdminCount;

                // Apply pagination to the filtered non-admin users query
                var users = await nonAdminUsersQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Map fetched database entities to DTOs
                var userDtos = users.Select(u => new GetUserDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Mobile = u.Mobile,
                    IsAdmin = u.IsAdmin ?? false,
                    IsBanned = u.IsBanned
                }).ToList();

                return new PaginationResult<GetUserDto>
                {
                    Items = userDtos,
                    TotalCount = totalCountExcludingAdmin, // Corrected total count excluding admin
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving users.", ex);
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
                // user.UserId = await IdGenerator.GenerateIdAsync<User>(_dbContext);
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
                // Retrieve the user entity with the specified ID from the database.
                var userToDelete = await _dbContext.Users.FindAsync(userId);
                if (userToDelete != null)
                {
                    // Retrieve orders associated with the user
                    var ordersToDelete = await _dbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
                    foreach (var order in ordersToDelete)
                    {
                        // Retrieve order products associated with the order
                        var orderProductsToDelete = await _dbContext.OrderProducts.Where(op => op.OrderId == order.OrderId).ToListAsync();
                        // Remove associated order products
                        _dbContext.OrderProducts.RemoveRange(orderProductsToDelete);
                    }
                    // Remove associated orders
                    _dbContext.Orders.RemoveRange(ordersToDelete);

                    // Remove the user from the database and save changes.
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