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
        public async Task<IActionResult> Create(CreateFooterDto dto)
        {
            var footer =_mapper.Map<Contact>(dto);

            await _context.Contacts.AddAsync(footer);
            await _context.SaveChangesAsync();
            return StatusCode(201, new {message ="Created"});
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var footers =await _context.Contacts.ToListAsync();

            var dto =_mapper.Map<List<ResultFooterDto>>(footers);

            return Ok(dto);
        }

         [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return Ok(new { message = "Footer Bulunamadı" });
            }

            var dto = _mapper.Map<GetByIdFooterDto>(contact);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateFooterDto dto)
        {
            var footer = await _context.Contacts.FindAsync(dto.Id);

            if (footer == null)
            {
                return Ok(new { message = "Footer Bulunamadı" });
            }

            _mapper.Map(dto, footer);

            _context.Contacts.Update(footer);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Updated" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var footer = await _context.Contacts.FindAsync(id);

            if (footer == null)
            {
                return Ok(new { message = "Footer Bulunamadı" });
            }

            _context.Contacts.Remove(footer);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted" });

        }
    }
}