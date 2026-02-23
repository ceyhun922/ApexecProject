using ApexWebAPI.DTOs.DepartmentDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<ResultDepartmentDto>> GetAllAsync(string lang);
        Task<GetByIdDepartmentDto?> GetByIdAsync(string lang, int id);
        Task CreateAsync(CreateDepartmentDto dto);
        Task UpdateAsync(UpdateDepartmentDto dto);
        Task DeleteAsync(int id);
    }
}
