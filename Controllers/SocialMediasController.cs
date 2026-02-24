using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.SocialMediaDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialMediasController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public SocialMediasController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultSocialMediaDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultSocialMediaDto>> Get()
        {
            var item = await _context.SocialMedias!.FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Sosial media tapılmadı" });

            return Ok(_mapper.Map<ResultSocialMediaDto>(item));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateSocialMediaDto dto)
        {
            var existing = await _context.SocialMedias!.ToListAsync();
            _context.SocialMedias!.RemoveRange(existing);

            var item = _mapper.Map<SocialMedia>(dto);
            item.CreatedDate = DateTime.UtcNow;
            await _context.SocialMedias.AddAsync(item);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Sosial media yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateSocialMediaDto dto)
        {
            var item = await _context.SocialMedias!.FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Sosial media tapılmadı" });

            _mapper.Map(dto, item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Sosial media yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var item = await _context.SocialMedias!.FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Sosial media tapılmadı" });

            _context.SocialMedias.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Sosial media silindi" });
        }
    }
}
