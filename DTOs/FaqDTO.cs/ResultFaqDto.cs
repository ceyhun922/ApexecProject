namespace ApexWebAPI.DTOs.FaqDTO.cs
{
    public class ResultFaqDto
    {
         public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool Status { get; set; }
    }
}