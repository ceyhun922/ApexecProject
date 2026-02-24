using System.ComponentModel;

namespace ApexWebAPI.DTOs.AboutCounterDTOs
{
    public class CreateAboutCounterDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public string? Text1Az { get; set; }
        public string? Text1En { get; set; }
        public string? Text1Ru { get; set; }
        public string? Text1Tr { get; set; }
        public string? Text2Az { get; set; }
        public string? Text2En { get; set; }
        public string? Text2Ru { get; set; }
        public string? Text2Tr { get; set; }
        public string? Text3Az { get; set; }
        public string? Text3En { get; set; }
        public string? Text3Ru { get; set; }
        public string? Text3Tr { get; set; }
        public string? Text4Az { get; set; }
        public string? Text4En { get; set; }
        public string? Text4Ru { get; set; }
        public string? Text4Tr { get; set; }
    }
}
