using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.LayoutDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class LayoutsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LayoutsController> _localizer;

        public LayoutsController(ApexDbContext context, IMapper mapper, IStringLocalizer<LayoutsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultLayoutDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultLayoutDto>> Get([FromRoute] string lang)
        {
            var item = await _context.Layouts!
                .Include(l => l.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<ResultLayoutDto>(item);
            

            var translation = item.Translations!
                .FirstOrDefault(t => t.Language == lang)
                ?? item.Translations!.FirstOrDefault(t => t.Language == "az");

            dto.FooterText = translation?.FooterText;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateLayoutDto dto)
        {
            var existing = await _context.Layouts!
                .Include(l => l.Translations)
                .ToListAsync();

            foreach (var e in existing)
            {
                if (e.Translations != null)
                    _context.LayoutTranslations!.RemoveRange(e.Translations);
            }
            _context.Layouts!.RemoveRange(existing);

            var item = new Layout
            {
                Logo = dto.Logo,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<LayoutTranslation>
                {
                    new() { Language = "az", FooterText = dto.FooterTextAz },
                    new() { Language = "en", FooterText = dto.FooterTextEn },
                    new() { Language = "ru", FooterText = dto.FooterTextRu },
                    new() { Language = "tr", FooterText = dto.FooterTextTr },
                }
            };

            await _context.Layouts.AddAsync(item);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateLayoutDto dto)
        {
            var item = await _context.Layouts!
                .Include(l => l.Translations)
                .FirstOrDefaultAsync(l => l.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.Logo = dto.Logo;
            item.Status = dto.Status;

            var langs = new[] {
                ("az", dto.FooterTextAz),
                ("en", dto.FooterTextEn),
                ("ru", dto.FooterTextRu),
                ("tr", dto.FooterTextTr)
            };

            foreach (var (l, footerText) in langs)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == l);
                if (t != null) t.FooterText = footerText;
                else item.Translations!.Add(new LayoutTranslation { Language = l, FooterText = footerText });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var item = await _context.Layouts!
                .Include(l => l.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            if (item.Translations != null)
                _context.LayoutTranslations!.RemoveRange(item.Translations);

            _context.Layouts.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
