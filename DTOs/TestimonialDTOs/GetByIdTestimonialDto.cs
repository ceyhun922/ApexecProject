namespace ApexWebAPI.DTOs.TestimonialDTOs
{
    public class GetByIdTestimonialDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? ImageUrl { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}