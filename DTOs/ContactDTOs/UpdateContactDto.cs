namespace ApexWebAPI.DTOs.ContactDTOs
{
    public class UpdateContactDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }

    }
}