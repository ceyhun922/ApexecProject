namespace ApexWebAPI.Entities
{
    public class Hero
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public bool Status { get; set; }

        public ICollection<HeroTranslation>? Translations {get;set;}

    }
}