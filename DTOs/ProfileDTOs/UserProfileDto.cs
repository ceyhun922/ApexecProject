namespace ApexWebAPI.DTOs.ProfileDTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string? ProfileImageUrl { get; set; }
    }
}