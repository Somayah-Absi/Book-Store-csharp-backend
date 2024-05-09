
using System.Text.Json.Serialization;
using Backend.Models;
namespace Backend.Dtos
{

  public class ProductDto
  {
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    [JsonIgnore]
    public string? ProductSlug { get; set; }
    [JsonIgnore]
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    [JsonIgnore]
    public string? ProductImage { get; set; }
    [JsonIgnore]
    public int ProductQuantityInStock { get; set; }
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }
    public int? CategoryId { get; set; }
  }

  public class OrderedProductDto
  {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
  }

  public class OrderCreationDto
  {
    public Order NewOrder { get; set; }
    public List<OrderedProductDto> Products { get; set; }
  }



  // public class ProductCreateDto
  // {
  //   public string ProductName { get; set; } = null!;
  //   public string? ProductSlug { get; set; }
  //   public string ProductDescription { get; set; } = null!;
  //   public decimal ProductPrice { get; set; }
  //   public string? ProductImage { get; set; }
  //   public int ProductQuantityInStock { get; set; }
  //   public int? CategoryId { get; set; }
  // }
}







