using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CoursesHeaderDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class CoursesHeadersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public CoursesHeadersController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultCoursesHeaderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultCoursesHeaderDto>> Get([FromRoute] string lang)
        {
            var item = await _context.CoursesHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Courses header tapılmadı" });

            var translation = item.Translations?.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations?.FirstOrDefault(t => t.Language == "az");

            var dto = _mapper.Map<ResultCoursesHeaderDto>(item);
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateCoursesHeaderDto dto)
        {
            var existing = await _context.CoursesHeaders!
                .Include(x => x.Translations)
                .ToListAsync();

            foreach (var e in existing)
            {
                if (e.Translations != null)
                    _context.CoursesHeaderTranslations!.RemoveRange(e.Translations);
            }
            _context.CoursesHeaders!.RemoveRange(existing);

            var item = new CoursesHeader
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                Translations = new List<CoursesHeaderTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.CoursesHeaders.AddAsync(item);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Courses header yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateCoursesHeaderDto dto)
        {
            var item = await _context.CoursesHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Courses header tapılmadı" });

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
                else item.Translations!.Add(new CoursesHeaderTranslation { Language = l, Title = title, SubTitle = subtitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Courses header yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var item = await _context.CoursesHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Courses header tapılmadı" });

            if (item.Translations != null)
                _context.CoursesHeaderTranslations!.RemoveRange(item.Translations);

            _context.CoursesHeaders.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Courses header silindi" });
        }
    }
}
