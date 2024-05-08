
namespace Backend.Dtos
{
    public class GetUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public bool? IsAdmin { get; set; } = false;
        public bool? IsBanned { get; set; } = false;
    }
}

