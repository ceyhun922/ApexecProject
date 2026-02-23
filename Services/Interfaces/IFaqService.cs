using ApexWebAPI.DTOs.FaqDTO.cs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IFaqService
    {
        Task<IEnumerable<ResultFaqDto>> GetAllAsync(string lang);
        Task<GetByIdFaqDto?> GetByIdAsync(string lang, int id);
        Task CreateAsync(CreateFaqDto dto);
        Task UpdateAsync(UpdateFaqDto dto);
        Task DeleteAsync(int id);
    }
}
