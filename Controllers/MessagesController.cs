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
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Contacts.ToListAsync();

            var dto = _mapper.Map<List<ResultMessageDto>>(messages);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdMessage(int id)
        {
            var message = await _context.Contacts.FindAsync(id);

            if (message == null)
            {
                return Ok(new { message = "NotFount" });
            }

            var mapper = _mapper.Map<GetByIdMessageDto>(message);
            return Ok(mapper);
        }

        [HttpPost("message-create")]
        public async Task<IActionResult> SubmitMessage([FromBody] CreateMessageDto messageDto)
        {

            var dto = _mapper.Map<Contact>(messageDto);

            _context.Contacts.Add(dto);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message successfully submitted" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateMessageDto dto)
        {
            var message = await _context.Contacts.FindAsync(dto.Id);

            if (message == null)
            {
                return Ok(new { message = "NotFount" });
            }

            _mapper.Map(dto, message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Message successfully updated" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Contacts.FindAsync(id);

            if (message == null)
            {
                return Ok(new { message = "NotFount" });
            }

            _context.Contacts.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message successfully deleted" });
        }

    }
}
