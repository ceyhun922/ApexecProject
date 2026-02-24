namespace ApexWebAPI.DTOs.PlanningDTOs
{
    public class ResultPlanningDto
    {
        public int Id { get; set; }
        public string? Option1 { get; set; }
        public string? Option1Title { get; set; }
        public string? Option2 { get; set; }
        public string? Option2Title { get; set; }
        public string? Option3 { get; set; }
        public string? Option3Title { get; set; }
        public string? Option4 { get; set; }
        public string? Option4Title { get; set; }
        public bool Checkbox1 { get; set; }
        public bool Checkbox2 { get; set; }
        public bool Checkbox3 { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
