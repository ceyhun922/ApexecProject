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
        public string? FbUrl { get; set; } = "https://www.facebook.com/";
        public string? InstaUrl { get; set; } = "https://www.instagram.com/";
        public string? LnUrl { get; set; } = "https://www.linkedin.com/company/";
        public string? XUrl { get; set; } = "https://x.com/";

        public string? Title {get;set;}
        public string? SubTitle {get;set;}
        public string? ImageUrl {get;set;}

        public string? OtherUrl { get; set; }
        public string? Message { get; set; }
    }
}