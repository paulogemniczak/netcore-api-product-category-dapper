namespace Gemniczak.Domain.Entities
{
    public class Product
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductCategoryId { get; set; } = 0;
    }
}
