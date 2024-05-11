using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Category
    {
        [Key]
        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters.")]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "Category slug is required.")]
        [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "Category slug can only contain lowercase letters, numbers, and hyphens.")]
        public string CategorySlug { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Category description cannot exceed 100 characters.")]
        public string? CategoryDescription { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationships
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
