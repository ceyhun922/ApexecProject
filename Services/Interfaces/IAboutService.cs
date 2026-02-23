using ApexWebAPI.DTOs.AboutDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IAboutService
    {
        Task<IEnumerable<ResultAboutDto>> GetAllAsync(string lang);
        Task<GetByIdAboutDto?> GetByIdAsync(string lang, int id);
        Task CreateAsync(CreateAboutDto dto);
        Task UpdateAsync(UpdateAboutDto dto);
        Task DeleteAsync(int id);
    }
}
