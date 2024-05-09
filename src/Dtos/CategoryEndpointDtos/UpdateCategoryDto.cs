using System.Text.Json.Serialization;

namespace Backend.Dtos
{
    public class UpdateCategoryDto
    {
        [JsonIgnore]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
    }
}