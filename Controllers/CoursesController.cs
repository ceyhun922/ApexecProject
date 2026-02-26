using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CourseDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Infrastructure.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CoursesController> _localizer;
        private readonly HtmlSanitizerService _htmlSanitizerService;

        public CoursesController(ApexDbContext context, IMapper mapper, IStringLocalizer<CoursesController> localizer, HtmlSanitizerService htmlSanitizerService)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _htmlSanitizerService = htmlSanitizerService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ResultCourseDto>), 200)]
        public async Task<ActionResult<List<ResultCourseDto>>> GetAll([FromRoute] string lang)
        {
            var items = await _context.Courses!
                .Include(c => c.Translations)
                .Where(c => c.Status)
                .ToListAsync();

            var result = items.Select(c =>
            {
                var dto = _mapper.Map<ResultCourseDto>(c);
                var t = c.Translations?.FirstOrDefault(t => t.Language == lang)
                    ?? c.Translations?.FirstOrDefault(t => t.Language == "az");
                dto.Title = t?.Title;
                dto.SubTitle = t?.SubTitle;
                dto.Description = t?.Description;
                return dto;
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdCourseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdCourseDto>> GetById([FromRoute] string lang, int id)
        {
            var item = await _context.Courses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdCourseDto>(item);
            var t = item.Translations?.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations?.FirstOrDefault(t => t.Language == "az");
            dto.Title = t?.Title;
            dto.SubTitle = t?.SubTitle;
            dto.Description = t?.Description;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            var item = new Course
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<CourseTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz, Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionAz) },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn, Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionEn) },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu, Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionRu) },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr, Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionTr) }
                }
            };

            await _context.Courses!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateCourseDto dto)
        {
            var item = await _context.Courses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.ImageUrl = dto.ImageUrl;
            item.Status = dto.Status;

            var langs = new[]
            {
                ("az", dto.TitleAz, dto.SubTitleAz, _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionAz)),
                ("en", dto.TitleEn, dto.SubTitleEn, _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionEn)),
                ("ru", dto.TitleRu, dto.SubTitleRu, _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionRu)),
                ("tr", dto.TitleTr, dto.SubTitleTr, _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionTr))
            };

            foreach (var (language, title, subTitle, description) in langs)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Title = title;
                    t.SubTitle = subTitle;
                    t.Description = description;
                }
                else
                {
                    item.Translations!.Add(new CourseTranslation
                    {
                        Language = language,
                        Title = title,
                        SubTitle = subTitle,
                        Description = description
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
            var item = await _context.Courses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Courses.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
