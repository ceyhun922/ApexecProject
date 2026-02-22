namespace ApexWebAPI.Entities
{
    public class EducationLevelTranslation
    {
        public int Id { get; set; }
        public string? Language { get; set; }
        public string? Name { get; set; }
        public int EducationLevelId {get;set;}
        public EducationLevel EducationLevel {get;set;}
    }
}