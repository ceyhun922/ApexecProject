using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FivePProgramHeaderDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class FivePProgramHeadersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public FivePProgramHeadersController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultFivePProgramHeaderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultFivePProgramHeaderDto>> Get([FromRoute] string lang)
        {
            var item = await _context.FivePProgramHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "5P program header tapılmadı" });

            var translation = item.Translations?.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations?.FirstOrDefault(t => t.Language == "az");

            var dto = _mapper.Map<ResultFivePProgramHeaderDto>(item);
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateFivePProgramHeaderDto dto)
        {
            var existing = await _context.FivePProgramHeaders!
                .Include(x => x.Translations)
                .ToListAsync();

            foreach (var e in existing)
            {
                if (e.Translations != null)
                    _context.FivePProgramHeaderTranslations!.RemoveRange(e.Translations);
            }
            _context.FivePProgramHeaders!.RemoveRange(existing);

            var item = new FivePProgramHeader
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                Translations = new List<FivePProgramHeaderTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.FivePProgramHeaders.AddAsync(item);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "5P program header yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateFivePProgramHeaderDto dto)
        {
            var item = await _context.FivePProgramHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "5P program header tapılmadı" });

            item.ImageUrl = dto.ImageUrl;
            item.Status = dto.Status;

            var langs = new[] {
                ("az", dto.TitleAz, dto.SubTitleAz),
                ("en", dto.TitleEn, dto.SubTitleEn),
                ("ru", dto.TitleRu, dto.SubTitleRu),
                ("tr", dto.TitleTr, dto.SubTitleTr)
            };

            foreach (var (l, title, subtitle) in langs)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == l);
                if (t != null) { t.Title = title; t.SubTitle = subtitle; }
                else item.Translations!.Add(new FivePProgramHeaderTranslation { Language = l, Title = title, SubTitle = subtitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "5P program header yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var item = await _context.FivePProgramHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "5P program header tapılmadı" });

            if (item.Translations != null)
                _context.FivePProgramHeaderTranslations!.RemoveRange(item.Translations);

            _context.FivePProgramHeaders.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "5P program header silindi" });
        }
    }
}
