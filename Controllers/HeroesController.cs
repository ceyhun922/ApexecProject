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
        public async Task<IActionResult> GetAll(string lang)
        {
            var hero = await _context.Heroes
                .Include(f => f.Translations)
                .ToListAsync();

            var result = hero.Select(f =>
            {
                var dto = _mapper.Map<ResultHeroDto>(f);
                dto.Title = f.Translations.FirstOrDefault(t => t.Language == lang)?.Title
                            ?? f.Translations.FirstOrDefault(t => t.Language == "az")?.Title;
                dto.SubTitle = f.Translations.FirstOrDefault(t => t.Language == lang)?.SubTitle
                               ?? f.Translations.FirstOrDefault(t => t.Language == "az")?.SubTitle;
                return dto;
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string lang, int id)
        {
            var hero = await _context.Heroes
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdHeroDto>(hero);
            dto.Title = hero.Translations.FirstOrDefault(t => t.Language == lang)?.Title
                        ?? hero.Translations.FirstOrDefault(t => t.Language == "az")?.Title;
            dto.SubTitle = hero.Translations.FirstOrDefault(t => t.Language == lang)?.SubTitle
                           ?? hero.Translations.FirstOrDefault(t => t.Language == "az")?.SubTitle;

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHeroDto dto)
        {
            var hero = _mapper.Map<Hero>(dto);

            hero.VideoUrl =dto.VideoUrl;
            hero.Translations = new List<HeroTranslation>
            {
                new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
            };

            await _context.Heroes.AddAsync(hero);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateHeroDto dto)
        {
            var hero = await _context.Heroes
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _mapper.Map(dto, hero);

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { "az", (dto.TitleAz, dto.SubTitleAz) },
                { "en", (dto.TitleEn, dto.SubTitleEn) },
                { "ru", (dto.TitleRu, dto.SubTitleRu) },
                { "tr", (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var translation = hero.Translations.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = title;
                    translation.SubTitle = subTitle;
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
            hero.VideoUrl = dto.VideoUrl;

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
 