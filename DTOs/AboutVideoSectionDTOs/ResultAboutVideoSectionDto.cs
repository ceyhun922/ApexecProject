namespace ApexWebAPI.DTOs.AboutVideoSectionDTOs
{
    public class ResultAboutVideoSectionDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? YouTubeUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
