
using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos
{

    public class OrderedProductDto
    {
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
    }

    public class OrderCreationDto
    {
        public Order NewOrder { get; set; } = null!;
        public List<OrderedProductDto> Products { get; set; } = null!;
    }

}