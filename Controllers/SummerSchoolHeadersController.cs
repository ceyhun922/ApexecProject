using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.SummerSchoolHeaderDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class SummerSchoolHeadersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public SummerSchoolHeadersController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultSummerSchoolHeaderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultSummerSchoolHeaderDto>> Get([FromRoute] string lang)
        {
            var item = await _context.SummerSchoolHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Summer school header tapılmadı" });

            var translation = item.Translations?.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations?.FirstOrDefault(t => t.Language == "az");

            var dto = _mapper.Map<ResultSummerSchoolHeaderDto>(item);
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateSummerSchoolHeaderDto dto)
        {
            var existing = await _context.SummerSchoolHeaders!
                .Include(x => x.Translations)
                .ToListAsync();

            foreach (var e in existing)
            {
                if (e.Translations != null)
                    _context.SummerSchoolHeaderTranslations!.RemoveRange(e.Translations);
            }
            _context.SummerSchoolHeaders!.RemoveRange(existing);

            var item = new SummerSchoolHeader
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                Translations = new List<SummerSchoolHeaderTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.SummerSchoolHeaders.AddAsync(item);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Summer school header yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateSummerSchoolHeaderDto dto)
        {
            var item = await _context.SummerSchoolHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Summer school header tapılmadı" });

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
                else item.Translations!.Add(new SummerSchoolHeaderTranslation { Language = l, Title = title, SubTitle = subtitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Summer school header yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var item = await _context.SummerSchoolHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Summer school header tapılmadı" });

            if (item.Translations != null)
                _context.SummerSchoolHeaderTranslations!.RemoveRange(item.Translations);

            _context.SummerSchoolHeaders.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Summer school header silindi" });
        }
    }
}
