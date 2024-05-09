

namespace Backend.Dtos
{
    public class OrderProductDto
    {
        public int OrderProductId { get; set; }
    
    public int? Quantity { get; set; }



    public int? ProductId { get; set; }



    public virtual ProductDto? Product { get; set; }


    }
}
