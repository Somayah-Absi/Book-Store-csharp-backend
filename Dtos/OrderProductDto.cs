using System.Text.Json.Serialization;
using Backend.Models;

namespace Backend.Dtos
{
    public class OrderProductDto
    {
        public int OrderProductId { get; set; }
    
    public int? Quantity { get; set; }



    public int? ProductId { get; set; }



    public virtual Product? Product { get; set; }


    }
}
