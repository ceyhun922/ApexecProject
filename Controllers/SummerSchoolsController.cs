using ApexWebAPI.Concrete;
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
        public async Task<IActionResult> GetAll([FromRoute] string lang)
        {
            var schools = await _context.SummerSchools.Include(s => s.Translations).Where(s => s.Status).ToListAsync();
            var result = schools.Select(s =>
            {
                var dto = _mapper.Map<ResultSummerSchoolDto>(s);
                dto.Title = s.Translations.FirstOrDefault(s => s.Language == lang)?.Title
                    ?? s.Translations.FirstOrDefault(s => s.Language == "az")?.Title;
                dto.SubTitle =s.Translations.FirstOrDefault(s=>s.Language ==lang).SubTitle
                    ?? s.Translations.FirstOrDefault(s=>s.SubTitle =="az").SubTitle;

                return dto;
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSummerSchoolDto dto)
        {
            var schools =_mapper.Map<SummerSchool>(dto);

            schools.CountryId =dto.CountryId;
            schools.ImageUrl =dto.ImageUrl;
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
    }
}