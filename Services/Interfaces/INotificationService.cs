using ApexWebAPI.DTOs.NotificationDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string title, string body, string type, int? referenceId = null);
        Task<List<NotificationDto>> GetAllAsync();
        Task<int> GetUnreadCountAsync();
        Task<bool> MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync();
        Task<bool> DeleteAsync(int id);
    }
}
