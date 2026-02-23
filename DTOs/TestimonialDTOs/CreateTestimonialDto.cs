using System.ComponentModel;
namespace ApexWebAPI.DTOs.TestimonialDTOs
{
    public class CreateTestimonialDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public string? FullName { get; set; }
        public string? ImageUrl { get; set; }
        public string? CommentAz { get; set; }
        public string? CommentEn { get; set; }
        public string? CommentRu { get; set; }
        public string? CommentTr { get; set; }
    }
}