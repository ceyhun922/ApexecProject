using ApexWebAPI.Common;
using ApexWebAPI.DTOs.FeatureDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IHeroService
    {
        Task<IEnumerable<ResultHeroDto>> GetAllAsync(string lang);
        Task<GetByIdHeroDto?> GetByIdAsync(string lang, int id);
        Task<int> CreateAsync(string lang, CreateHeroDto dto);
        Task UpdateAsync(string lang, UpdateHeroDto dto);
        Task DeleteAsync(int id);
    }
}
