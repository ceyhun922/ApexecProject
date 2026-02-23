using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.MessageDTOs.cs;
using ApexWebAPI.Entities;
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

        public MessagesController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet("messages-list")]
        [ProducesResponseType(typeof(List<ResultMessageDto>), 200)]
        public async Task<ActionResult<List<ResultMessageDto>>> GetMessages()
        {
            var messages = await _context.Contacts.ToListAsync();

            var dto = _mapper.Map<List<ResultMessageDto>>(messages);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdMessageDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdMessageDto>> GetByIdMessage(int id)
        {
            var message = await _context.Contacts.FindAsync(id);

            if (message == null)
                return NotFound(new { message = "Mesaj tapılmadı" });

            var dto = _mapper.Map<GetByIdMessageDto>(message);
            return Ok(dto);
        }

        [HttpPost("message-create")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> SubmitMessage([FromBody] CreateMessageDto messageDto)
        {
            var entity = _mapper.Map<Contact>(messageDto);
            _context.Contacts.Add(entity);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Mesaj uğurla göndərildi" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateMessageDto dto)
        {
            var message = await _context.Contacts.FindAsync(dto.Id);

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
            var message = await _context.Contacts.FindAsync(id);

            if (message == null)
                return NotFound(new { message = "Mesaj tapılmadı" });

            _context.Contacts.Remove(message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Mesaj uğurla silindi" });
        }

    }
}
