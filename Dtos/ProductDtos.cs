
using Backend.Models;
namespace Backend.Dtos
{

  public class ProductDto
  {
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ProductSlug { get; set; }
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    public string? ProductImage { get; set; }
    public int ProductQuantityInStock { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
  }

  public class ProductCreateDto
  {
    public string ProductName { get; set; } = null!;
    public string? ProductSlug { get; set; }
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    public string? ProductImage { get; set; }
    public int ProductQuantityInStock { get; set; }
    public int? CategoryId { get; set; }
  }
}







