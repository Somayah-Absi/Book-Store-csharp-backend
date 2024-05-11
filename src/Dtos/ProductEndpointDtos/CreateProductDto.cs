using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "Product description is required")]
        public string ProductDescription { get; set; } = null!;

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0")]
        public decimal ProductPrice { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProductImage { get; set; } = null!;

        [Required(ErrorMessage = "Product quantity in stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Product quantity must be a positive number")]
        public int ProductQuantityInStock { get; set; }
        [Required(ErrorMessage = "Product must belong to an existing category by category ID")]
        public int CategoryId { get; set; }

    }
}
