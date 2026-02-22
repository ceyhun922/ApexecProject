using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FeatureDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<HeroesController> _localizer;

        public HeroesController(ApexDbContext context, IMapper mapper, IStringLocalizer<HeroesController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] string lang)
        {
            var heroes = await _context.Heroes
                .Include(h => h.Translations)
                .Where(h => h.Status)
                .ToListAsync();

            var result = heroes.Select(h =>
            {
                var dto = _mapper.Map<ResultHeroDto>(h);

                var translation = h.Translations
                    .FirstOrDefault(t => t.Language == lang)
                    ?? h.Translations.FirstOrDefault(t => t.Language == "az");

                dto.Title = translation?.Title;
                dto.SubTitle = translation?.SubTitle;

                return dto;
            });

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string lang, int id)
        {
            var hero = await _context.Heroes
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hero == null)
                return NotFound();

            var dto = _mapper.Map<GetByIdHeroDto>(hero);

            var translation = hero.Translations
                .FirstOrDefault(t => t.Language == lang)
                ?? hero.Translations.FirstOrDefault(t => t.Language == "az");

            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;

            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateHeroDto dto)
        {
            var hero = new Hero
            {
                VideoUrl = dto.VideoUrl,
                Status = true,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<HeroTranslation>
        {
            new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
            new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
            new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
            new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
        }
            };

            await _context.Heroes.AddAsync(hero);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = hero.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateHeroDto dto)
        {
            var hero = await _context.Heroes
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == dto.Id);

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            if (!string.IsNullOrEmpty(dto.VideoUrl))
                hero.VideoUrl = dto.VideoUrl;
            hero.Status = dto.Status;

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
    {
        { "az", (dto.TitleAz, dto.SubTitleAz) },
        { "en", (dto.TitleEn, dto.SubTitleEn) },
        { "ru", (dto.TitleRu, dto.SubTitleRu) },
        { "tr", (dto.TitleTr, dto.SubTitleTr) }
    };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var t = hero.Translations.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Title = title;
                    t.SubTitle = subTitle;
                }
                else
                {
                    hero.Translations.Add(new HeroTranslation
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
        public async Task<IActionResult> Delete(int id)
        {
            var hero = await _context.Heroes
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
