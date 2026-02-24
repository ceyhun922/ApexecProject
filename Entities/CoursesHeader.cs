namespace ApexWebAPI.Entities
{
    public class CoursesHeader
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<CoursesHeaderTranslation>? Translations { get; set; }
    }
}
