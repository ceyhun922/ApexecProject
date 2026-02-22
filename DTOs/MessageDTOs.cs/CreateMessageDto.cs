namespace ApexWebAPI.DTOs.MessageDTOs.cs
{
    public class CreateMessageDto
    {
         public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
    }
}