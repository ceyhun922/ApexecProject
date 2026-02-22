namespace ApexWebAPI.DTOs.SummerSchoolDTOs
{
    public class GetByIdSchoolSummerDto
    {
        
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public int CountryId { get; set; }
    }
}