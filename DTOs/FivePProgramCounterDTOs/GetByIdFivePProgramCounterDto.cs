namespace ApexWebAPI.DTOs.FivePProgramCounterDTOs
{
    public class GetByIdFivePProgramCounterDto
    {
        public int Id { get; set; }
        public string? Count1 { get; set; }
        public string? Count2 { get; set; }
        public string? Count3 { get; set; }
        public string? Count4 { get; set; }
        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }
        public string? Text4 { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
