namespace ApexWebAPI.DTOs.AdminSearchDTOs
{
    public class AdminSearchResultDto
    {
        public List<AdminSearchItemDto> Messages { get; set; } = new();
        public List<AdminSearchItemDto> SummerSchools { get; set; } = new();
        public List<AdminSearchItemDto> Faqs { get; set; } = new();
        public List<AdminSearchItemDto> Countries { get; set; } = new();
        public List<AdminSearchItemDto> Departments { get; set; } = new();
        public List<AdminSearchItemDto> Abouts { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class AdminSearchItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
    }
}
