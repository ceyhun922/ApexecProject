using System.ComponentModel;

namespace ApexWebAPI.DTOs.PlanningDTOs
{
    public class CreatePlanningDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public string? BadgeAz { get; set; }
        public string? BadgeEn { get; set; }
        public string? BadgeRu { get; set; }
        public string? BadgeTr { get; set; }

        public string? TitleAz { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }

        public string? SubTitleAz { get; set; }
        public string? SubTitleEn { get; set; }
        public string? SubTitleRu { get; set; }
        public string? SubTitleTr { get; set; }
    }
}
