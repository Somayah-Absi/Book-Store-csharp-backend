namespace Backend.Dtos
{
    public class GetProductWithCategoryDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSlug { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public int ProductQuantityInStock { get; set; }
    }
}