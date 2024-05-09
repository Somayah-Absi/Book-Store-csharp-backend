

using System.Text.Json.Serialization;
using Backend.Models;

namespace Backend.Dtos
{
    public class OrderProductDto
    {
        [JsonIgnore]

        public int OrderProductId { get; set; }

        public int? Quantity { get; set; }
        [JsonIgnore]
        public int? ProductId { get; set; }
        public virtual ProductDto? Product { get; set; }
    }

}
