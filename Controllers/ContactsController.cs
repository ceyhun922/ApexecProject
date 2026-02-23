using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.ContactDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public ContactsController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultContactDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultContactDto>> Get()
        {
            var contact = await _context.Contacts!.FirstOrDefaultAsync();

            if (contact == null)
                return NotFound(new { message = "Əlaqə məlumatı tapılmadı" });

            return Ok(_mapper.Map<ResultContactDto>(contact));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateContactDto dto)
        {
            var existing = await _context.Contacts!.ToListAsync();
            _context.Contacts!.RemoveRange(existing);

            var contact = _mapper.Map<Contact>(dto);
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateContactDto dto)
        {
            var contact = await _context.Contacts!.FirstOrDefaultAsync();

            if (contact == null)
                return NotFound(new { message = "Əlaqə tapılmadı" });

            _mapper.Map(dto, contact);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var contact = await _context.Contacts!.FirstOrDefaultAsync();

            if (contact == null)
                return NotFound(new { message = "Əlaqə tapılmadı" });

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}
