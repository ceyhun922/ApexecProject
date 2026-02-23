using ApexWebAPI.DTOs.ContactInfoDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ResultContactInfoDto>> GetAllAsync();
        Task<GetByIdContactDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateContactInfoDto dto);
        Task UpdateAsync(UpdateContactInfoDto dto);
        Task DeleteAsync(int id);
    }
}
