namespace ApexWebAPI.DTOs.DepartmentDTOs
{
    public class UpdateDepartmentDto
    {
           public int Id { get; set; }
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameTr { get; set; }
        public string? NameRu { get; set; }
        public int EducationLevelId {get;set;}
    }
}