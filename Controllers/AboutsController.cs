using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.AboutDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class AboutsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AboutsController> _localizer;

        public AboutsController(ApexDbContext context, IMapper mapper, IStringLocalizer<AboutsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultAboutDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultAboutDto>> Get(string lang)
        {
            var about = await _context.Abouts!
                .Include(x => x.AboutTranslations)
                .FirstOrDefaultAsync();

            if (about == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<ResultAboutDto>(about);
            dto.Title = about.AboutTranslations!.FirstOrDefault(t => t.Language == lang)?.Title
                        ?? about.AboutTranslations!.FirstOrDefault(t => t.Language == "az")?.Title;
            dto.SubTitle = about.AboutTranslations!.FirstOrDefault(t => t.Language == lang)?.SubTitle
                           ?? about.AboutTranslations!.FirstOrDefault(t => t.Language == "az")?.SubTitle;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateAboutDto dto)
        {
            var existing = await _context.Abouts!
                .Include(x => x.AboutTranslations)
                .ToListAsync();

            _context.Abouts!.RemoveRange(existing);

            var about = new About
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                AboutTranslations = new List<AboutTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.Abouts.AddAsync(about);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = _localizer["Created"].Value });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateAboutDto dto)
        {
            var about = await _context.Abouts!
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync();

            if (about == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            about.ImageUrl = dto.ImageUrl;
            about.Status = dto.Status;

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { "az", (dto.TitleAz, dto.SubTitleAz) },
                { "en", (dto.TitleEn, dto.SubTitleEn) },
                { "ru", (dto.TitleRu, dto.SubTitleRu) },
                { "tr", (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var translation = about.AboutTranslations!.FirstOrDefault(t => t.Language == language);
                if (translation != null) { translation.Title = title; translation.SubTitle = subTitle; }
                else about.AboutTranslations!.Add(new AboutTranslation { Language = language, Title = title, SubTitle = subTitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var about = await _context.Abouts!
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync();

            if (about == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Abouts.Remove(about);
            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
