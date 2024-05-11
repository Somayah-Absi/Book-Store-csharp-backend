using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(30, ErrorMessage = "First Name must be less than 30 characters")]
        [MinLength(4, ErrorMessage = "First Name must be more than 4 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(30, ErrorMessage = "Last Name must be less than 30 characters")]
        public string? LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        public string? Password { get; set; }

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string? Mobile { get; set; }
        public bool? IsAdmin { get; set; } = false;
        public bool? IsBanned { get; set; } = false;
    }
}
