namespace ApexWebAPI.Entities
{
    public class Country
    {
         public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<EducationLevel>? EducationLevels {get;set;}
        public ICollection<CountryTranslation>? CountryTranslations {get;set;}

        public ICollection<SummerSchool>? SummerSchools { get; set; }
    }
}