using System.ComponentModel;
namespace ApexWebAPI.DTOs.DepartmentDTOs
{
    public class CreateDepartmentDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameTr { get; set; }
        public string? NameRu { get; set; }
        public int EducationLevelId {get;set;}
    }
}