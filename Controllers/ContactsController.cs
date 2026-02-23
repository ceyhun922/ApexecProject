using System.Threading.Tasks;
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

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateContactDto dto)
        {
            var contacts = _mapper.Map<Contact>(dto);

            contacts.ImageUrl = dto.ImageUrl;

            await _context.Contacts.AddAsync(contacts);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Yaradıldı" });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResultContactDto>), 200)]
        public async Task<ActionResult<List<ResultContactDto>>> GetAll()
        {
            var contacts = await _context.Contacts.ToListAsync();

            var dto = _mapper.Map<List<ResultContactDto>>(contacts);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdContactDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdContactDto>> GetById(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound(new { message = "Əlaqə tapılmadı" });

            var dto = _mapper.Map<GetByIdContactDto>(contact);

            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateContactDto dto)
        {
            var contact = await _context.Contacts.FindAsync(dto.Id);

            if (contact == null)
                return NotFound(new { message = "Əlaqə tapılmadı" });

            _mapper.Map(dto, contact);
            contact.ImageUrl = dto.ImageUrl;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Yeniləndi" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound(new { message = "Əlaqə tapılmadı" });

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}