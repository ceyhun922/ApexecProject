namespace ApexWebAPI.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int EducationLevelId {get;set;}
        public EducationLevel? EducationLevel {get;set;}
        public ICollection<DepartmentTranslation>? DepartmentTranslations {get;set;}
        public ICollection<University>? Universities { get; set; }
    }
}