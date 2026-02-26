namespace ApexWebAPI.DTOs.CourseDTOs
{
    public class GetByIdCourseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
