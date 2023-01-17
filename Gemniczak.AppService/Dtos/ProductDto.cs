using System.Text.Json.Serialization;

namespace Gemniczak.AppService.Dtos
{
    public class ProductDto
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public CategoryDto ProductCategory { get; set; } = new CategoryDto();

        [JsonIgnore]
        public int? ProductCategoryId { get; set; } = 0;
    }
}
