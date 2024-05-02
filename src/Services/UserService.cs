using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class UserService
    {
        private readonly EcommerceSdaContext _dbContext;

        public UserService(EcommerceSdaContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                List<User> users = new List<User>();
                var dataList = _dbContext.Users.ToList();
                dataList.ForEach(row => users.Add(new User
                {
                    UserId = row.UserId,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Email = row.Email,
                    Mobile = row.Mobile,
                    Password = row.Password,
                    CreatedAt = row.CreatedAt,
                }));
                return users;
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while retrieving users.", ex);
            }
        }

        public User? GetUserById(int id)
        {
            try
            {
                return _dbContext.Users.Find(id);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException($"An error occurred while retrieving user with ID {id}.", ex);
            }
        }

        public User CreateUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new ApplicationException("An error occurred while creating user.", ex);
            }
        }

        public User? UpdateUser(int id, User user)
        {
            try
            {
                var existingUser = _dbContext.Users.Find(id);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
                    existingUser.LastName = user.LastName ?? existingUser.LastName;
                    existingUser.Email = user.Email ?? existingUser.Email;
                    existingUser.Mobile = user.Mobile ?? existingUser.Mobile;
                    existingUser.Password = user.Password ?? existingUser.Password;
                    existingUser.IsAdmin = user.IsAdmin;
                    existingUser.IsBanned = user.IsBanned;
                    // Update other properties as needed
                    _dbContext.SaveChanges();
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
                throw new ApplicationException($"An error occurred while updating user with ID {id}.", ex);
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                var userToDelete = _dbContext.Users.Find(id);
                if (userToDelete != null)
                {
                    _dbContext.Users.Remove(userToDelete);
                    _dbContext.SaveChanges();
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
                throw new ApplicationException($"An error occurred while deleting user with ID {id}.", ex);
            }
        }
    }
}
