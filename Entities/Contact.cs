namespace ApexWebAPI.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? Email { get; set; }
        public string? Adress { get; set; }
        public string? Education { get; set; }
        public int ClassOrYear { get; set; }
        public string? Message { get; set; }
    }
}