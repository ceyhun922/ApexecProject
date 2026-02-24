namespace ApexWebAPI.DTOs.SocialMediaDTOs
{
    public class ResultSocialMediaDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string? Url { get; set; }
        public bool Status { get; set; }
    }
}
