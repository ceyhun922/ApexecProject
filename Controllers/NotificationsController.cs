using ApexWebAPI.DTOs.NotificationDTOs;
using ApexWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApexWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationDto>), 200)]
        public async Task<ActionResult<List<NotificationDto>>> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> GetUnreadCount()
        {
            var count = await _notificationService.GetUnreadCountAsync();
            return Ok(new { count });
        }

        [HttpPut("{id}/mark-read")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            if (!result) return NotFound(new { message = "Bildiriş tapılmadı" });
            return Ok(new { message = "Bildiriş oxundu olaraq işarələndi" });
        }

        [HttpPut("mark-all-read")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            await _notificationService.MarkAllAsReadAsync();
            return Ok(new { message = "Bütün bildirişlər oxundu olaraq işarələndi" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _notificationService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Bildiriş tapılmadı" });
            return Ok(new { message = "Bildiriş silindi" });
        }
    }
}
