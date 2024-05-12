using System.ComponentModel.DataAnnotations;

namespace auth.Dtos
{
    public class LoginDto
    {
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
        public string? Email { set; get; }
        public string? Password { set; get; }

    }
}