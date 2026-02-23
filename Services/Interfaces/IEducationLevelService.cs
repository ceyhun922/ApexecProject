using ApexWebAPI.DTOs.EducationLevelDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IEducationLevelService
    {
        Task<IEnumerable<ResultEducationLevelDto>> GetAllAsync(string lang);
        Task<GetByIdEducationLevelDto?> GetByIdAsync(string lang, int id);
        Task CreateAsync(CreateEducationLevelDto dto);
        Task UpdateAsync(UpdateEducationLevelDto dto);
        Task DeleteAsync(int id);
    }
}
