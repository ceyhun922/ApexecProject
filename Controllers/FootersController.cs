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
            var footer = await _context.Contacts!.FirstOrDefaultAsync();

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            return Ok(_mapper.Map<ResultFooterDto>(footer));
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateFooterDto dto)
        {
            var footer = await _context.Contacts!.FirstOrDefaultAsync();

            if (footer == null)
                return NotFound(new { message = "Footer tapılmadı" });

            _mapper.Map(dto, footer);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Footer uğurla yeniləndi" });
        }
    }
}
