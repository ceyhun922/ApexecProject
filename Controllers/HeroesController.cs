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
        [ProducesResponseType(typeof(IEnumerable<ResultHeroDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultHeroDto>>> GetAll([FromRoute] string lang)
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
        [ProducesResponseType(typeof(GetByIdHeroDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdHeroDto>> GetById([FromRoute] string lang, int id)
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



        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromForm] CreateHeroDto dto)
        {
            string? videoUrl = null;

            if (dto.Video != null && dto.Video.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Video.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Video.CopyToAsync(stream);

                videoUrl = $"/videos/{fileName}";
            }

            var hero = new Hero
            {
                VideoUrl = videoUrl,
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
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromForm] UpdateHeroDto dto)
        {
            var hero = await _context.Heroes
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == dto.Id);

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            if (dto.Video != null && dto.Video.Length > 0)
            {
                if (!string.IsNullOrEmpty(hero.VideoUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hero.VideoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Video.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Video.CopyToAsync(stream);

                hero.VideoUrl = $"/videos/{fileName}";
            }

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
    }
}
