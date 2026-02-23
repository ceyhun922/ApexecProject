namespace ApexWebAPI.DTOs.ContactHeaderDTOs
{
    public class ResultContactHeaderDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
    }
}
