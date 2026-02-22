namespace ApexWebAPI.DTOs.MessageDTOs.cs
{
    public class ResultMessageDto
    {
         public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }

    }
}