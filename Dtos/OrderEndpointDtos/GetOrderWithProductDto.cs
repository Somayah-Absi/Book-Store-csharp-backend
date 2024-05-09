
namespace Backend.Dtos
{

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public decimal ProductPrice { get; set; }

        public int? CategoryId { get; set; }
    }
}