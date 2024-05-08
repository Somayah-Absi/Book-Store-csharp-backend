
using System.Text.Json.Serialization;
using Backend.Dtos;

namespace Backend.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
     
      
    }
}
