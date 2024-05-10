
using Backend.Models;

namespace Backend.Dtos
{

    public class OrderedProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderCreationDto
    {
        public Order NewOrder { get; set; } = null!;
        public List<OrderedProductDto> Products { get; set; } = null!;
    }

}