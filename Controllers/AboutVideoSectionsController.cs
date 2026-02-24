using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.AboutVideoSectionDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class AboutVideoSectionsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AboutVideoSectionsController> _localizer;

        public AboutVideoSectionsController(ApexDbContext context, IMapper mapper, IStringLocalizer<AboutVideoSectionsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdAboutVideoSectionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdAboutVideoSectionDto>> Get([FromRoute] string lang)
        {
            var item = await _context.AboutVideoSections!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdAboutVideoSectionDto>(item);

            var translation = item.Translations!
                .FirstOrDefault(t => t.Language == lang)
                ?? item.Translations!.FirstOrDefault(t => t.Language == "az");

            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateAboutVideoSectionDto dto)
        {
            var existing = await _context.AboutVideoSections!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync();

            if (existing != null)
                _context.AboutVideoSections.Remove(existing);

            var item = new AboutVideoSection
            {
                YouTubeUrl = dto.YouTubeUrl,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<AboutVideoSectionTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.AboutVideoSections!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateAboutVideoSectionDto dto)
        {
            var item = await _context.AboutVideoSections!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.YouTubeUrl = dto.YouTubeUrl;
            item.Status = dto.Status;

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { "az", (dto.TitleAz, dto.SubTitleAz) },
                { "en", (dto.TitleEn, dto.SubTitleEn) },
                { "ru", (dto.TitleRu, dto.SubTitleRu) },
                { "tr", (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Title = title;
                    t.SubTitle = subTitle;
                }
                else
                {
                    item.Translations!.Add(new AboutVideoSectionTranslation
                    {
                        Language = language,
                        Title = title,
                        SubTitle = subTitle
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.AboutVideoSections!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.AboutVideoSections.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
