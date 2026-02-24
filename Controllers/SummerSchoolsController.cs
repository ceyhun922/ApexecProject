using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.SummerSchoolDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class SummerSchoolsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SummerSchoolsController> _localizer;

        public SummerSchoolsController(ApexDbContext context, IMapper mapper, IStringLocalizer<SummerSchoolsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultSummerSchoolDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultSummerSchoolDto>>> GetAll([FromRoute] string lang)
        {
            var schools = await _context.SummerSchools
                .Include(s => s.Translations)
                .Where(s => s.Status)
                .ToListAsync();

            var result = schools.Select(s =>
            {
                var dto = _mapper.Map<ResultSummerSchoolDto>(s);
                dto.Title = s.Translations.FirstOrDefault(t => t.Language == lang)?.Title
                    ?? s.Translations.FirstOrDefault(t => t.Language == "az")?.Title;
                dto.SubTitle = s.Translations.FirstOrDefault(t => t.Language == lang)?.SubTitle
                    ?? s.Translations.FirstOrDefault(t => t.Language == "az")?.SubTitle;
                return dto;
            });

            return Ok(result);
        }

        [HttpGet("countries")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultCountryDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultCountryDto>>> GetCountriesWithSummerSchools([FromRoute] string lang)
        {
            var countries = await _context.SummerSchools
                .Where(s => s.Status)
                .Include(s => s.Country)
                    .ThenInclude(c => c!.CountryTranslations)
                .Select(s => s.Country!)
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

        [HttpGet("by-country/{countryId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultSummerSchoolDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ResultSummerSchoolDto>>> GetByCountry([FromRoute] string lang, [FromRoute] int countryId)
        {
            var schools = await _context.SummerSchools
                .Include(s => s.Translations)
                .Where(s => s.Status && s.CountryId == countryId)
                .ToListAsync();

            if (!schools.Any())
                return NotFound(new { message = "Bu ölkə üçün yay məktəbi tapılmadı" });

            var result = schools.Select(s =>
            {
                var dto = _mapper.Map<ResultSummerSchoolDto>(s);
                dto.Title = s.Translations.FirstOrDefault(t => t.Language == lang)?.Title
                    ?? s.Translations.FirstOrDefault(t => t.Language == "az")?.Title;
                dto.SubTitle = s.Translations.FirstOrDefault(t => t.Language == lang)?.SubTitle
                    ?? s.Translations.FirstOrDefault(t => t.Language == "az")?.SubTitle;
                return dto;
            });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateSummerSchoolDto dto)
        {
            var schools = _mapper.Map<SummerSchool>(dto);

            schools.CountryId = dto.CountryId;
            schools.ImageUrl = dto.ImageUrl;
            schools.Translations = new List<SummerSchoolTranslation>
            {
                new() { Language = "az", Title = dto.TitleAz},
                new() { Language = "en", Title = dto.TitleEn},
                new() { Language = "ru", Title = dto.TitleRu},
                new() { Language = "tr", Title = dto.TitleTr},
                new() { Language = "az", SubTitle = dto.SubTitleAz},
                new() { Language = "en", SubTitle = dto.SubTitleEn},
                new() { Language = "ru", SubTitle = dto.SubTitleRu},
                new() { Language = "tr", SubTitle = dto.SubTitleTr}
            };

            await _context.SummerSchools.AddAsync(schools);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateSummerSchoolDto dto)
        {
            var school = await _context.SummerSchools
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == dto.Id);

            if (school == null)
                return NotFound(new { message = _localizer["NotFount"].Value });

            _mapper.Map(dto, school);

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
                {
                    { "az", (dto.TitleAz, dto.SubTitleAz) },
                    { "en", (dto.TitleEn, dto.SubTitleEn) },
                    { "ru", (dto.TitleRu, dto.SubTitleRu) },
                    { "tr", (dto.TitleTr, dto.SubTitleTr) }
                };

            foreach (var (language, value) in translations)

            {
                var translation = school.Translations.FirstOrDefault(t => t.Language == language);

                if (translation != null)
                {
                    translation.Title = value.Title;
                    translation.SubTitle = value.SubTitle;
                }
                else
                {
                    school.Translations.Add(new SummerSchoolTranslation
                    {
                        Language = language,
                        Title = value.Title,
                        SubTitle = value.SubTitle
                    });
                }
            }

            school.ImageUrl = dto.ImageUrl;
            await _context.SaveChangesAsync();


            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete("{id}")]

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var schools = await _context.SummerSchools!
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schools == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.SummerSchools.Remove(schools);
            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}