namespace Gemniczak.Domain.Filter
{
    public class BaseFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? OrderBy { get; set; }
        public bool? IsAsc { get; set; }
    }
}
