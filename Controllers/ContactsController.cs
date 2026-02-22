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
        public async Task<IActionResult> Create(CreateContactDto dto)
        {
            var contacts = _mapper.Map<Contact>(dto);

            contacts.ImageUrl =dto.ImageUrl;

            await _context.Contacts.AddAsync(contacts);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Created" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _context.Contacts.ToListAsync();

            var dto = _mapper.Map<List<ResultContactDto>>(contacts);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return Ok(new { message = "Contact Bulunamadı" });
            }

            var dto = _mapper.Map<GetByIdContactDto>(contact);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateContactDto dto)
        {
            var contact = await _context.Contacts.FindAsync(dto.Id);

            if (contact == null)
            {
                return Ok(new { message = "Contact Bulunamadı" });
            }

            _mapper.Map(dto, contact);
            contact.ImageUrl =dto.ImageUrl;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Updated" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return Ok(new { message = "Contact Bulunamadı" });
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted" });

        }
    }
}