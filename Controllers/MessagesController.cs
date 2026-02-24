using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.MessageDTOs.cs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public MessagesController(ApexDbContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }



        [HttpGet("messages-list")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ResultMessageDto>), 200)]
        public async Task<ActionResult<List<ResultMessageDto>>> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();

            var dto = _mapper.Map<List<ResultMessageDto>>(messages);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdMessageDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdMessageDto>> GetByIdMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return NotFound(new { message = "Mesaj tapılmadı" });

            var dto = _mapper.Map<GetByIdMessageDto>(message);
            return Ok(dto);
        }

        [HttpPost("message-create")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> SubmitMessage([FromBody] CreateMessageDto messageDto)
        {
            var entity = _mapper.Map<Message>(messageDto);
            _context.Messages.Add(entity);
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendMessageNotificationAsync(
                    entity.FullName ?? "",
                    entity.Email ?? "",
                    entity.PhoneNumber ?? "",
                    entity.Messagee ?? ""
                );
            }
            catch (Exception ex)
            {
                _ = ex;
            }

            return StatusCode(201, new { message = "Mesaj uğurla göndərildi" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateMessageDto dto)
        {
            var message = await _context.Messages.FindAsync(dto.Id);

            if (message == null)
                return NotFound(new { message = "Mesaj tapılmadı" });

            _mapper.Map(dto, message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Mesaj uğurla yeniləndi" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return NotFound(new { message = "Mesaj tapılmadı" });

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Mesaj uğurla silindi" });
        }

    }
}
