using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class EducationLevelsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<EducationLevelsController> _localizer;

        public EducationLevelsController(ApexDbContext context, IMapper mapper, IStringLocalizer<EducationLevelsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResultEducationLevelDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultEducationLevelDto>>> GetAll(string lang, [FromQuery] int? countryId = null)
        {
            var query = _context.EducationLevels
                .Include(el => el.EducationLevelTranslations)
                .AsQueryable();

            if (countryId.HasValue)
                query = query.Where(el => el.CountryId == countryId.Value);

            var educations = await query.ToListAsync();

            var result = educations.Select(c =>
            {
                var dto = _mapper.Map<ResultEducationLevelDto>(c);
                dto.Name = c.EducationLevelTranslations.FirstOrDefault(t => t.Language == lang)?.Name
                    ?? c.EducationLevelTranslations.FirstOrDefault(t => t.Language == "az")?.Name;
                return dto;
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdEducationLevelDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdEducationLevelDto>> GetByIdEducationLevel(string lang, int id)
        {
            var education = await _context.EducationLevels.Include(ed => ed.EducationLevelTranslations).FirstOrDefaultAsync(ed => ed.Id == id);
            if (education == null)
                return NotFound(new { message = _localizer["NotFound"].Value });
            var dto = _mapper.Map<GetByIdEducationLevelDto>(education);
            dto.Name = education.EducationLevelTranslations.FirstOrDefault(ed => ed.Language == lang)?.Name
                ?? education.EducationLevelTranslations.FirstOrDefault(ed => ed.Language == "az")?.Name;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateEducationLevelDto dto)
        {
            var educations = _mapper.Map<EducationLevel>(dto);

            educations.EducationLevelTranslations = new List<EducationLevelTranslation>
            {
                 new EducationLevelTranslation { Language ="az", Name =dto.NameAz},
                new EducationLevelTranslation { Language ="en", Name =dto.NameEn},
                new EducationLevelTranslation { Language ="tr", Name =dto.NameTr},
                new EducationLevelTranslation { Language ="ru", Name =dto.NameRu}
            };

            await _context.AddAsync(educations);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Created" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var education = await _context.EducationLevels
                .Include(ed => ed.EducationLevelTranslations)
                .FirstOrDefaultAsync(ed => ed.Id == id);

            if (education == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.EducationLevels.Remove(education);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateEducationLevelDto dto)
        {
            var education = await _context.EducationLevels
                .Include(ed => ed.EducationLevelTranslations)
                .FirstOrDefaultAsync(ed => ed.Id == dto.Id);

            if (education == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _mapper.Map(dto, education);

            var translations = new Dictionary<string, string>
            {
                ["az"] = dto.NameAz,
                ["en"] = dto.NameEn,
                ["ru"] = dto.NameRu,
                ["tr"] = dto.NameTr
            };

            foreach (var (language, name) in translations)
            {
               var translation = education.EducationLevelTranslations
                    .FirstOrDefault(t => t.Language == language);

                if (translation == null)
                {
                    education.EducationLevelTranslations.Add(new EducationLevelTranslation
                    {
                        EducationLevelId = education.Id,
                        Language = language,
                        Name = name
                    });
                }
                else
                {
                    translation.Name = name;
                }

            }
                await _context.SaveChangesAsync();
                 return Ok(new { message = _localizer["Updated"].Value });
        }
    }
}