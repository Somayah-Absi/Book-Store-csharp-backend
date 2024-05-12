using Backend.Models;

namespace auth.Data
{
    public interface IUserRepository
    {
        // Method to create a new user in the data store
        User Create(User user);

        // Method to retrieve a user by their email address
        User GetByEmail(string email);

        // Method to retrieve a user by their unique identifier (ID)
        User GetById(int id);
    }
}