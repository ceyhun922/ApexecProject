using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAll(string lang)
        {
            var about =await _context.Abouts.Include(x=>x.AboutTranslations).ToListAsync();

            var result = about.Select(f =>
            {
                var dto = _mapper.Map<ResultAboutDto>(f);
                dto.Title = f.AboutTranslations.FirstOrDefault(t => t.Language == lang)?.Title
                            ?? f.AboutTranslations.FirstOrDefault(t => t.Language == "az")?.Title;
                dto.SubTitle = f.AboutTranslations.FirstOrDefault(t => t.Language == lang)?.SubTitle
                               ?? f.AboutTranslations.FirstOrDefault(t => t.Language == "az")?.SubTitle;
                return dto;
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string lang, int id)
        {
            var about = await _context.Abouts
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (about == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdAboutDto>(about);
            dto.Title = about.AboutTranslations.FirstOrDefault(t => t.Language == lang)?.Title
                        ?? about.AboutTranslations.FirstOrDefault(t => t.Language == "az")?.Title;
            dto.SubTitle = about.AboutTranslations.FirstOrDefault(t => t.Language == lang)?.SubTitle
                           ?? about.AboutTranslations.FirstOrDefault(t => t.Language == "az")?.SubTitle;

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAboutDto dto)
        {
            var about = _mapper.Map<About>(dto);

            about.ImageUrl = dto.ImageUrl;
            about.AboutTranslations = new List<AboutTranslation>
            {
                new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
            };

            await _context.Abouts.AddAsync(about);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAboutDto dto)
        {
            var about = await _context.Abouts
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (about == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _mapper.Map(dto, about);

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { "az", (dto.TitleAz, dto.SubTitleAz) },
                { "en", (dto.TitleEn, dto.SubTitleEn) },
                { "ru", (dto.TitleRu, dto.SubTitleRu) },
                { "tr", (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var translation = about.AboutTranslations.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = title;
                    translation.SubTitle = subTitle;
                }
                else
                {
                    about.AboutTranslations.Add(new AboutTranslation
                    {
                        Language = language,
                        Title = title,
                        SubTitle = subTitle
                    });
                }
            }
            about.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }
         [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var about = await _context.Abouts
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (about == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Abouts.Remove(about);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}