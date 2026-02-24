namespace ApexWebAPI.DTOs.SocialMediaDTOs
{
    public class UpdateSocialMediaDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string? Url { get; set; }
        public bool Status { get; set; }
    }
}
