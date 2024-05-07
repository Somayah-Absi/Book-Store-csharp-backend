using Backend.Models;

namespace Backend.Dtos
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public ICollection<ProductDto>? Products { get; set; }
    }

}