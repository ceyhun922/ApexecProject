using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultHeroDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultHeroDto>> Get([FromRoute] string lang)
        {
            var hero = await _context.Heroes!
                .Include(h => h.Translations)
                .FirstOrDefaultAsync();

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<ResultHeroDto>(hero);
            var translation = hero.Translations!
                .FirstOrDefault(t => t.Language == lang)
                ?? hero.Translations!.FirstOrDefault(t => t.Language == "az");

            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;

            return Ok(dto);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromForm] CreateHeroDto dto)
        {
            var existing = await _context.Heroes!
                .Include(h => h.Translations)
                .ToListAsync();

            foreach (var h in existing)
            {
                if (!string.IsNullOrEmpty(h.VideoUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", h.VideoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
            }

            _context.Heroes!.RemoveRange(existing);

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
                Status = dto.Status,
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
            var hero = await _context.Heroes!
                .Include(h => h.Translations)
                .FirstOrDefaultAsync();

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
                var t = hero.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null) { t.Title = title; t.SubTitle = subTitle; }
                else hero.Translations!.Add(new HeroTranslation { Language = language, Title = title, SubTitle = subTitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var hero = await _context.Heroes!
                .Include(h => h.Translations)
                .FirstOrDefaultAsync();

            if (hero == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            if (!string.IsNullOrEmpty(hero.VideoUrl))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hero.VideoUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
