using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.NotificationDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Hubs;
using ApexWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class NotificationService : INotificationService
    {
        private readonly ApexDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(ApexDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(string title, string body, string type, int? referenceId = null)
        {
            var notification = new Notification
            {
                Title = title,
                Body = body,
                Type = type,
                ReferenceId = referenceId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            var dto = MapToDto(notification);
            await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", dto);
        }

        public async Task<List<NotificationDto>> GetAllAsync()
        {
            var notifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return notifications.Select(MapToDto).ToList();
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.Notifications.CountAsync(n => !n.IsRead);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group("Admins").SendAsync("NotificationRead", id);
            return true;
        }

        public async Task MarkAllAsReadAsync()
        {
            var unread = await _context.Notifications.Where(n => !n.IsRead).ToListAsync();
            unread.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group("Admins").SendAsync("AllNotificationsRead");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        private static NotificationDto MapToDto(Notification n) => new()
        {
            Id = n.Id,
            Title = n.Title,
            Body = n.Body,
            Type = n.Type,
            IsRead = n.IsRead,
            ReferenceId = n.ReferenceId,
            CreatedDate = n.CreatedDate
        };
    }
}
