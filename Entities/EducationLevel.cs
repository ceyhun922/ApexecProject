namespace ApexWebAPI.Entities
{
    public class EducationLevel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int CountryId {get;set;}

        public Country? Country {get;set;}
        public ICollection<Department>? Departments {get;set;}
        public ICollection<University>? Universities { get; set; }
        public ICollection<EducationLevelTranslation> EducationLevelTranslations {get;set;}
    }
}