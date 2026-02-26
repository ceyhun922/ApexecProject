namespace ApexWebAPI.DTOs.PlanningDTOs
{
    public class ResultPlanningDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Badge { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
    }
}
