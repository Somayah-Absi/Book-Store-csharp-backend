
namespace Backend.Dtos
{
    public class OrderProductDto
    {


        public int? Quantity { get; set; }

        public virtual ProductDto? Product { get; set; }
    }

}
