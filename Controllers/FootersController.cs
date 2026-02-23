using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FooterDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FootersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public FootersController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateFooterDto dto)
        {
            var footer =_mapper.Map<Contact>(dto);

            await _context.Contacts.AddAsync(footer);
            await _context.SaveChangesAsync();
            return StatusCode(201, new {message ="Created"});
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResultFooterDto>), 200)]
        public async Task<ActionResult<List<ResultFooterDto>>> GetAll()
        {
            var footers =await _context.Contacts.ToListAsync();

            var dto =_mapper.Map<List<ResultFooterDto>>(footers);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdFooterDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdFooterDto>> GetById(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound(new { message = "Footer tapılmadı" });

            var dto = _mapper.Map<GetByIdFooterDto>(contact);
            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateFooterDto dto)
        {
            var footer = await _context.Contacts.FindAsync(dto.Id);

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            _mapper.Map(dto, footer);
            _context.Contacts.Update(footer);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Footer uğurla yeniləndi" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var footer = await _context.Contacts.FindAsync(id);

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            _context.Contacts.Remove(footer);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Footer uğurla silindi" });
        }
    }
}