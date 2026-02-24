namespace ApexWebAPI.DTOs.FivePProgramCounterDTOs
{
    public class ResultFivePProgramCounterDto
    {
        public int Id { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }
        public string? Text4 { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
