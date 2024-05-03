using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public partial class Category
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

        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
        public string? CategoryDescription { get; set; }

        [Display(Name = "Created At")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationships
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
