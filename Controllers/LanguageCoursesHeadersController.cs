using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.LanguageCoursesHeaderDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class LanguageCoursesHeadersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public LanguageCoursesHeadersController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultLanguageCoursesHeaderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultLanguageCoursesHeaderDto>> Get([FromRoute] string lang)
        {
            var item = await _context.LanguageCoursesHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Language courses header tapılmadı" });

            var translation = item.Translations?.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations?.FirstOrDefault(t => t.Language == "az");

            var dto = _mapper.Map<ResultLanguageCoursesHeaderDto>(item);
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateLanguageCoursesHeaderDto dto)
        {
            var existing = await _context.LanguageCoursesHeaders!
                .Include(x => x.Translations)
                .ToListAsync();

            foreach (var e in existing)
            {
                if (e.Translations != null)
                    _context.LanguageCoursesHeaderTranslations!.RemoveRange(e.Translations);
            }
            _context.LanguageCoursesHeaders!.RemoveRange(existing);

            var item = new LanguageCoursesHeader
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                Translations = new List<LanguageCoursesHeaderTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.LanguageCoursesHeaders.AddAsync(item);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Language courses header yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateLanguageCoursesHeaderDto dto)
        {
            var item = await _context.LanguageCoursesHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Language courses header tapılmadı" });

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
                else item.Translations!.Add(new LanguageCoursesHeaderTranslation { Language = l, Title = title, SubTitle = subtitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Language courses header yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var item = await _context.LanguageCoursesHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Language courses header tapılmadı" });

            if (item.Translations != null)
                _context.LanguageCoursesHeaderTranslations!.RemoveRange(item.Translations);

            _context.LanguageCoursesHeaders.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Language courses header silindi" });
        }
    }
}
