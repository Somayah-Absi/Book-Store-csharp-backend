

using System.Text.Json.Serialization;

namespace Backend.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = null!;
        public required string? Mobile { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        public bool? IsAdmin { get; set; } = false;
        public bool? IsBanned { get; set; } = false;
    }
}
