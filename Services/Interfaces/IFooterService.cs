using ApexWebAPI.DTOs.FooterDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IFooterService
    {
        Task<IEnumerable<ResultFooterDto>> GetAllAsync();
        Task<GetByIdFooterDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateFooterDto dto);
        Task UpdateAsync(UpdateFooterDto dto);
        Task DeleteAsync(int id);
    }
}
