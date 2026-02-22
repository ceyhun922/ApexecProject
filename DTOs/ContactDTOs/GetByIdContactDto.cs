namespace ApexWebAPI.DTOs.ContactDTOs
{
    public class GetByIdContactDto
    {
         public int Id { get; set; }
         public string? PhoneNumber { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }
    }
}