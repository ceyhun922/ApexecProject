namespace ApexWebAPI.DTOs.DepartmentDTOs
{
    public class GetByIdDepartmentDto
    {
           public int Id { get; set; }
        public string? Name { get; set; }
        public int EducationLevelId {get;set;}
    }
}