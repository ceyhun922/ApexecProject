namespace ApexWebAPI.Entities
{
    public class FivePProgramHeaderTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int FivePProgramHeaderId { get; set; }
        public FivePProgramHeader? FivePProgramHeader { get; set; }
    }
}
