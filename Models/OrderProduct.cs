using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Backend.Models;

public class OrderProduct
{
    [Key]
    [JsonIgnore]
    public int OrderProductId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 1.")]
    public int? Quantity { get; set; }
    public int? OrderId { get; set; }

    public int? ProductId { get; set; }
    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
