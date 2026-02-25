using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.LanguageCourseDTOs;
using ApexWebAPI.DTOs.LanguageDTOs;
using ApexWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class LanguageCoursesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IStringLocalizer<LanguageCoursesController> _localizer;

        public LanguageCoursesController(ApexDbContext context, IStringLocalizer<LanguageCoursesController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultLanguageCourseDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultLanguageCourseDto>>> GetAll(
            [FromRoute] string lang,
            [FromQuery] int? languageId,
            [FromQuery] int? countryId)
        {
            var query = _context.LanguageCourses!
                .Include(c => c.Translations)
                .Where(c => c.Status)
                .AsQueryable();

            if (languageId.HasValue)
                query = query.Where(c => c.LanguageId == languageId.Value);

            if (countryId.HasValue)
                query = query.Where(c => c.CountryId == countryId.Value);

            var courses = await query.ToListAsync();

            var result = courses.Select(c =>
            {
                var dto = new ResultLanguageCourseDto
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    Status = c.Status,
                    LanguageId = c.LanguageId,
                    CountryId = c.CountryId
                };
                var translation = c.Translations?.FirstOrDefault(t => t.Lang == lang)
                    ?? c.Translations?.FirstOrDefault(t => t.Lang == "az");
                dto.Title = translation?.Title;
                dto.SubTitle = translation?.SubTitle;
                return dto;
            });

            return Ok(result);
        }

        // Kurs olan dilleri döner (Dil seçin dropdown)
        [HttpGet("languages")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultLanguageDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultLanguageDto>>> GetLanguagesWithCourses([FromRoute] string lang)
        {
            var languages = await _context.LanguageCourses!
                .Where(c => c.Status)
                .Include(c => c.Language)
                    .ThenInclude(l => l!.LanguageTranslations)
                .Select(c => c.Language!)
                .Distinct()
                .ToListAsync();

            var result = languages.Select(l => new ResultLanguageDto
            {
                Id = l.Id,
                Status = l.Status,
                Name = l.LanguageTranslations?
                    .FirstOrDefault(t => t.Lang == lang)?.Name
                    ?? l.LanguageTranslations?.FirstOrDefault(t => t.Lang == "az")?.Name
            });

            return Ok(result);
        }

        // Seçilen dilde kurs olan ülkeleri döner (Ölkə seçin dropdown)
        [HttpGet("countries")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultCountryDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultCountryDto>>> GetCountriesByLanguage(
            [FromRoute] string lang,
            [FromQuery] int? languageId)
        {
            var query = _context.LanguageCourses!
                .Where(c => c.Status)
                .Include(c => c.Country)
                    .ThenInclude(c => c!.CountryTranslations)
                .AsQueryable();

            if (languageId.HasValue)
                query = query.Where(c => c.LanguageId == languageId.Value);

            var countries = await query
                .Select(c => c.Country!)
                .Distinct()
                .ToListAsync();

            var result = countries.Select(c => new ResultCountryDto
            {
                Id = c.Id,
                Name = c.CountryTranslations?
                    .FirstOrDefault(t => t.Language == lang)?.Name
                    ?? c.CountryTranslations?.FirstOrDefault(t => t.Language == "az")?.Name
            });

            return Ok(result);
        }

        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultLanguageCourseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultLanguageCourseDto>> GetDetail([FromRoute] string lang, int id)
        {
            var course = await _context.LanguageCourses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id && c.Status);

            if (course == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = new ResultLanguageCourseDto
            {
                Id = course.Id,
                ImageUrl = course.ImageUrl,
                Status = course.Status,
                LanguageId = course.LanguageId,
                CountryId = course.CountryId
            };
            var translation = course.Translations?.FirstOrDefault(t => t.Lang == lang)
                ?? course.Translations?.FirstOrDefault(t => t.Lang == "az");
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultLanguageCourseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultLanguageCourseDto>> GetById([FromRoute] string lang, int id)
        {
            var course = await _context.LanguageCourses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = new ResultLanguageCourseDto
            {
                Id = course.Id,
                ImageUrl = course.ImageUrl,
                Status = course.Status,
                LanguageId = course.LanguageId,
                CountryId = course.CountryId
            };
            var translation = course.Translations?.FirstOrDefault(t => t.Lang == lang)
                ?? course.Translations?.FirstOrDefault(t => t.Lang == "az");
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateLanguageCourseDto dto)
        {
            var course = new LanguageCourse
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                LanguageId = dto.LanguageId,
                CountryId = dto.CountryId,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<LanguageCourseTranslation>
                {
                    new() { Lang = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Lang = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Lang = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Lang = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr }
                }
            };

            await _context.LanguageCourses!.AddAsync(course);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = course.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateLanguageCourseDto dto)
        {
            var course = await _context.LanguageCourses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (course == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            course.ImageUrl = dto.ImageUrl;
            course.Status = dto.Status;
            course.LanguageId = dto.LanguageId;
            course.CountryId = dto.CountryId;

            var langs = new[]
            {
                ("az", dto.TitleAz, dto.SubTitleAz),
                ("en", dto.TitleEn, dto.SubTitleEn),
                ("ru", dto.TitleRu, dto.SubTitleRu),
                ("tr", dto.TitleTr, dto.SubTitleTr)
            };

            foreach (var (l, title, subTitle) in langs)
            {
                var t = course.Translations!.FirstOrDefault(x => x.Lang == l);
                if (t != null) { t.Title = title; t.SubTitle = subTitle; }
                else course.Translations!.Add(new LanguageCourseTranslation { Lang = l, Title = title, SubTitle = subTitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.LanguageCourses!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            if (course.Translations != null)
                _context.LanguageCourseTranslations!.RemoveRange(course.Translations);

            _context.LanguageCourses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
