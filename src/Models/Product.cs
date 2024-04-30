public class Product
{

    public Guid ProductId { get; set; }
      public required string ProductName { get; set; }
    public  required string ProductSlug { get; set; }
    public  required string ProductDescription { get; set; } = string.Empty;
    public required decimal ProductPrice { get; set; }
    public string ProductImage { get; set; } = string.Empty;
    public  required int ProductQuantityInStock { get; set; }
   public required Guid CategoryId { get; set; }
//   public Category? Category { get; set; }

  public DateTime CreatedAt { get; set; }
}


