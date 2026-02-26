namespace ApexWebAPI.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<CourseTranslation>? Translations { get; set; }
    }
}
