namespace ApexWebAPI.Entities
{
    public class AboutTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; }

        public string? Title { get; set; }
        public string? SubTitle { get; set; }

        public int AboutId {get;set;}
        public About? About {get;set;}

    }
}