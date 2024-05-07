using System.ComponentModel.DataAnnotations;


namespace Backend.Models;

public class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "First Name is Required")]
    [MaxLength(30, ErrorMessage = "First Name must be less than 30 characters")]
    [MinLength(4, ErrorMessage = "First Name must be more than 4 characters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last Name is Required")]
    [MaxLength(30, ErrorMessage = " Last Name must be less than 30 characters")]
    [MinLength(4, ErrorMessage = "Last Name must be more than 4 characters")]
    public string LastName { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    public required string Email { get; set; } = null!;

    [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits")]
    public required string? Mobile { get; set; }

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
    public required string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsAdmin { get; set; } = false;

    public bool? IsBanned { get; set; } = false;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
