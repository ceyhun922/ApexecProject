using ApexWebAPI.DTOs.ContactDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ResultContactDto>> GetAllAsync();
        Task<GetByIdContactDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateContactDto dto);
        Task UpdateAsync(UpdateContactDto dto);
        Task DeleteAsync(int id);
    }
}
