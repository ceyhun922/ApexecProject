using ApexWebAPI.DTOs.MessageDTOs.cs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<ResultMessageDto>> GetAllAsync();
        Task<GetByIdMessageDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateMessageDto dto);
        Task UpdateAsync(UpdateMessageDto dto);
        Task DeleteAsync(int id);
    }
}
