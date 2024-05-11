using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace auth.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(30, ErrorMessage = "First Name must be less than 30 characters")]
        [MinLength(4, ErrorMessage = "First Name must be more than 4 characters")]
        public string? FirstName { set; get; }
        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(30, ErrorMessage = " Last Name must be less than 30 characters")]
        public string? LastName { set; get; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
        public string? Email { set; get; }
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [Display(Name = "Created At")]
        public string? Password { set; get; }
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string? Mobile { set; get; }
        [JsonIgnore]
        public bool? IsAdmin { get; set; } = false;
        [JsonIgnore]
        public bool? IsBanned { get; set; } = false;
    }
}