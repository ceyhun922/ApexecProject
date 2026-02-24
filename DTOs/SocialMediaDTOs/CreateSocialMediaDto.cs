using System.ComponentModel;

namespace ApexWebAPI.DTOs.SocialMediaDTOs
{
    public class CreateSocialMediaDto
    {
        public string Key { get; set; } = null!;
        public string? Url { get; set; }
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
    }
}
