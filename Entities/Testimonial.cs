namespace ApexWebAPI.Entities
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public ICollection<TestimonialTranslation> Translations { get; set; }

    }
}