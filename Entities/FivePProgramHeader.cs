namespace ApexWebAPI.Entities
{
    public class FivePProgramHeader
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<FivePProgramHeaderTranslation>? Translations { get; set; }
    }
}
