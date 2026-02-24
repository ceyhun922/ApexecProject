using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.SocialMediaDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ResultSocialMediaDto>), 200)]
        public async Task<ActionResult<List<ResultSocialMediaDto>>> GetAll()
        {
            var items = await _context.SocialMedias!
                .Where(x => x.Status)
                .ToListAsync();

            return Ok(_mapper.Map<List<ResultSocialMediaDto>>(items));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultSocialMediaDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultSocialMediaDto>> GetById(int id)
        {
            var item = await _context.SocialMedias!.FindAsync(id);

            if (item == null)
                return NotFound(new { message = "Sosial media tapılmadı" });

            return Ok(_mapper.Map<ResultSocialMediaDto>(item));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateSocialMediaDto dto)
        {
            var item = _mapper.Map<SocialMedia>(dto);
            item.CreatedDate = DateTime.UtcNow;

            await _context.SocialMedias!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Sosial media yaradıldı", id = item.Id });
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateSocialMediaDto dto)
        {
            var item = await _context.SocialMedias!.FindAsync(dto.Id);

            if (item == null)
                return NotFound(new { message = "Sosial media tapılmadı" });

            _mapper.Map(dto, item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sosial media yeniləndi" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.SocialMedias!.FindAsync(id);

            if (item == null)
                return NotFound(new { message = "Sosial media tapılmadı" });

            _context.SocialMedias.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sosial media silindi" });
        }
    }
}
