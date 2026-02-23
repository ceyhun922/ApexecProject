using ApexWebAPI.DTOs.InformationDTOs;
using ApexWebAPI.DTOs.MessageDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IInformationService
    {
        Task<IEnumerable<ResultInformationDto>> GetAllAsync();
        Task<GetByIdInformationDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateInformationDto dto);
        Task UpdateAsync(UpdateInformationDto dto);
        Task DeleteAsync(int id);
    }
}
