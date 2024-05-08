namespace Backend.Dtos
{
    public class GetCategoryWithProductDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategorySlug { get; set; }
        public string? CategoryDescription { get; set; }
        public ICollection<GetProductWithCategoryDto>? Products { get; set; }
    }

}