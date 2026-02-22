namespace ApexWebAPI.DTOs.SearchDTOs
{
    public class SearchResultDto
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<SearchItemDto> Results { get; set; } = new();
    }
}