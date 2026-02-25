namespace ApexWebAPI.Entities
{
    public class University
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public int EducationLevelId { get; set; }
        public EducationLevel? EducationLevel { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public List<UniversityTranslation> Translations { get; set; } = new();
    }
}
