namespace ApexWebAPI.DTOs.CoursesHeaderDTOs
{
    public class ResultCoursesHeaderDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
    }
}
