namespace ApexWebAPI.DTOs.FaqDTO.cs
{
    public class GetByIdFaqDto
    {
         public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool Status { get; set; }
    }
}