using ApexWebAPI.DTOs.CountryDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<ResultCountryDto>> GetAllAsync(string lang);
        Task<GetByIdCountryDto?> GetByIdAsync(string lang, int id);
        Task CreateAsync(CreateCountryDto dto);
        Task UpdateAsync(UpdateCountryDto dto);
        Task DeleteAsync(int id);
    }
}
