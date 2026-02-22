namespace ApexWebAPI.Entities
{
    public class DepartmentTranslation
    {
        
        public int Id { get; set; }
        public string? Language { get; set; }
        public string? Name { get; set; }
        public int DepartmentId {get;set;}
        public Department? Department {get;set;}
    }
}