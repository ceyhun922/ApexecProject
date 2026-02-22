namespace ApexWebAPI.DTOs.InformationDTOs
{
    public class GetByIdInformationDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Education { get; set; }
        public int ClassOrYear { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}