using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.LanguageDTOs;
using ApexWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class LanguagesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IStringLocalizer<LanguagesController> _localizer;

        public LanguagesController(ApexDbContext context, IStringLocalizer<LanguagesController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultLanguageDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultLanguageDto>>> GetAll([FromRoute] string lang)
        {
            var items = await _context.Languages!
                .Include(l => l.LanguageTranslations)
                .Where(l => l.Status)
                .ToListAsync();

            var result = items.Select(l => new ResultLanguageDto
            {
                Id = l.Id,
                Status = l.Status,
                Name = l.LanguageTranslations?
                    .FirstOrDefault(t => t.Lang == lang)?.Name
                    ?? l.LanguageTranslations?.FirstOrDefault(t => t.Lang == "az")?.Name
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultLanguageDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultLanguageDto>> GetById([FromRoute] string lang, int id)
        {
            var item = await _context.Languages!
                .Include(l => l.LanguageTranslations)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            return Ok(new ResultLanguageDto
            {
                Id = item.Id,
                Status = item.Status,
                Name = item.LanguageTranslations?
                    .FirstOrDefault(t => t.Lang == lang)?.Name
                    ?? item.LanguageTranslations?.FirstOrDefault(t => t.Lang == "az")?.Name
            });
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateLanguageDto dto)
        {
            var item = new Language
            {
                Status = dto.Status,
                LanguageTranslations = new List<LanguageTranslation>
                {
                    new() { Lang = "az", Name = dto.NameAz },
                    new() { Lang = "en", Name = dto.NameEn },
                    new() { Lang = "ru", Name = dto.NameRu },
                    new() { Lang = "tr", Name = dto.NameTr }
                }
            };

            await _context.Languages!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateLanguageDto dto)
        {
            var item = await _context.Languages!
                .Include(l => l.LanguageTranslations)
                .FirstOrDefaultAsync(l => l.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.Status = dto.Status;

            var langs = new[] { ("az", dto.NameAz), ("en", dto.NameEn), ("ru", dto.NameRu), ("tr", dto.NameTr) };
            foreach (var (l, name) in langs)
            {
                var t = item.LanguageTranslations!.FirstOrDefault(x => x.Lang == l);
                if (t != null) t.Name = name;
                else item.LanguageTranslations!.Add(new LanguageTranslation { Lang = l, Name = name });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Languages!
                .Include(l => l.LanguageTranslations)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            if (item.LanguageTranslations != null)
                _context.LanguageTranslations!.RemoveRange(item.LanguageTranslations);

            _context.Languages.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
