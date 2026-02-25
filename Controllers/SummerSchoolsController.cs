using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.SummerSchoolDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ApexWebAPI.Infrastructure.Services;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class SummerSchoolsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SummerSchoolsController> _localizer;
        private readonly HtmlSanitizerService _htmlSanitizerService;

        public SummerSchoolsController(ApexDbContext context, IMapper mapper, IStringLocalizer<SummerSchoolsController> localizer, HtmlSanitizerService htmlSanitizerService)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _htmlSanitizerService = htmlSanitizerService;
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
                var t = s.Translations.FirstOrDefault(x => x.Language == lang)
                    ?? s.Translations.FirstOrDefault(x => x.Language == "az");
                var dto = _mapper.Map<ResultSummerSchoolDto>(s);
                dto.Title = t?.Title;
                dto.SubTitle = t?.SubTitle;
                dto.Description = t?.Description;
                return dto;
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdSchoolSummerDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdSchoolSummerDto>> GetById([FromRoute] string lang, int id)
        {
            var school = await _context.SummerSchools
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (school == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var t = school.Translations.FirstOrDefault(x => x.Language == lang)
                ?? school.Translations.FirstOrDefault(x => x.Language == "az");
            var dto = _mapper.Map<GetByIdSchoolSummerDto>(school);
            dto.Title = t?.Title;
            dto.SubTitle = t?.SubTitle;
            dto.Description = t?.Description;

            return Ok(dto);
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

        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultSummerSchoolDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultSummerSchoolDto>> GetDetail([FromRoute] string lang, [FromRoute] int id)
        {
            var school = await _context.SummerSchools
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == id && s.Status);

            if (school == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var td = school.Translations.FirstOrDefault(x => x.Language == lang)
                ?? school.Translations.FirstOrDefault(x => x.Language == "az");
            var dto = _mapper.Map<ResultSummerSchoolDto>(school);
            dto.Title = td?.Title;
            dto.SubTitle = td?.SubTitle;
            dto.Description = td?.Description;

            return Ok(dto);
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
                var tc = s.Translations.FirstOrDefault(x => x.Language == lang)
                    ?? s.Translations.FirstOrDefault(x => x.Language == "az");
                var dto = _mapper.Map<ResultSummerSchoolDto>(s);
                dto.Title = tc?.Title;
                dto.SubTitle = tc?.SubTitle;
                dto.Description = tc?.Description;
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
                new() { Language = "az", Title = _htmlSanitizerService.SanitizeHtmlContent(dto.TitleAz), SubTitle = _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleAz), Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionAz) },
                new() { Language = "en", Title = _htmlSanitizerService.SanitizeHtmlContent(dto.TitleEn), SubTitle = _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleEn), Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionEn) },
                new() { Language = "ru", Title = _htmlSanitizerService.SanitizeHtmlContent(dto.TitleRu), SubTitle = _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleRu), Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionRu) },
                new() { Language = "tr", Title = _htmlSanitizerService.SanitizeHtmlContent(dto.TitleTr), SubTitle = _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleTr), Description = _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionTr) }
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

            var translations = new[]
            {
                 ("az", _htmlSanitizerService.SanitizeHtmlContent(dto.TitleAz), _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleAz), _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionAz)),
                ("en", _htmlSanitizerService.SanitizeHtmlContent(dto.TitleEn), _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleEn), _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionEn)),
                ("ru", _htmlSanitizerService.SanitizeHtmlContent(dto.TitleRu), _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleRu), _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionRu)),
                ("tr", _htmlSanitizerService.SanitizeHtmlContent(dto.TitleTr), _htmlSanitizerService.SanitizeHtmlContent(dto.SubTitleTr), _htmlSanitizerService.SanitizeHtmlContent(dto.DescriptionTr))
            };

            foreach (var (language, title, subTitle, description) in translations)
            {
                var translation = school.Translations.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = title;
                    translation.SubTitle = subTitle;
                    translation.Description = description;
                }
                else
                {
                    school.Translations.Add(new SummerSchoolTranslation
                    {
                        Language = language,
                        Title = title,
                        SubTitle = subTitle,
                        Description = description
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