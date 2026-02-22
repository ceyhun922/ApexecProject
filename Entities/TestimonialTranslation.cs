namespace ApexWebAPI.Entities
{
    public class TestimonialTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } 
        public string? Comment { get; set; }
        public string? Position { get; set; }
        public int TestimonialId { get; set; }
        public Testimonial? Testimonial { get; set; }
    }
}