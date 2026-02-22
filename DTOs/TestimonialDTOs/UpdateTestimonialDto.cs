namespace ApexWebAPI.DTOs.TestimonialDTOs
{
    public class UpdateTestimonialDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? ImageUrl { get; set; }
        public string? CommentAz { get; set; }
        public string? CommentEn { get; set; }
        public string? CommentRu { get; set; }
        public string? CommentTr { get; set; }
        public bool Status { get; set; }
    }
}