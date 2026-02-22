namespace ApexWebAPI.Entities
{
    public class HeroTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } 
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int HeroId { get; set; }
        public Hero? Hero { get; set; }
    }
}