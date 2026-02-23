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

        [HttpGet]
        [ProducesResponseType(typeof(ResultFooterDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultFooterDto>> Get()
        {
            var footer = await _context.Footers!.FirstOrDefaultAsync();

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            return Ok(_mapper.Map<ResultFooterDto>(footer));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateFooterDto dto)
        {
            var existing = await _context.Footers!.ToListAsync();
            _context.Footers!.RemoveRange(existing);

            var footer = _mapper.Map<Footer>(dto);
            await _context.Footers.AddAsync(footer);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Footer yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateFooterDto dto)
        {
            var footer = await _context.Footers!.FirstOrDefaultAsync();

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            _mapper.Map(dto, footer);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Footer uğurla yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var footer = await _context.Footers!.FirstOrDefaultAsync();

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            _context.Footers.Remove(footer);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Footer silindi" });
        }
    }
}
