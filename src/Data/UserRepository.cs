using System.Linq;
using Backend.Models;


namespace auth.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly EcommerceSdaContext _appDbContext;

        // Constructor
        public UserRepository(EcommerceSdaContext context)
        {
            _appDbContext = context;
        }

        // Adds a new user to the database
        public User Create(User user)
        {
            _appDbContext.Users.Add(user);
            user.UserId = _appDbContext.SaveChanges();

            return user;
        }

        // Retrieves a user by email from the database
        public User GetByEmail(string email)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return user;
        }

        // Retrieves a user by ID from the database
        public User GetById(int id)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                throw new Exception($"User with id {id} not found.");
            }
            return user;
        }
    }

}