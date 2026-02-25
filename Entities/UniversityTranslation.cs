namespace ApexWebAPI.Entities
{
    public class UniversityTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }

        public int UniversityId { get; set; }
        public University University { get; set; } = null!;
    }
}
