using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.ContactInfoDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactInfosController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public ContactInfosController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultContactInfoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultContactInfoDto>> Get()
        {
            var contact = await _context.Contacts!.FirstOrDefaultAsync();

            if (contact == null)
                return NotFound(new { message = "Əlaqə məlumatı tapılmadı" });

            return Ok(_mapper.Map<ResultContactInfoDto>(contact));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateContactInfoDto dto)
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
        public async Task<IActionResult> Update(UpdateContactInfoDto dto)
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
