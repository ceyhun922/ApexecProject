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
        public async Task<IActionResult> GetAll(string lang)
        {
            var educations = await _context.EducationLevels.Include(el => el.EducationLevelTranslations).ToListAsync();

            var dto = _mapper.Map<List<ResultEducationLevelDto>>(educations);
            var result = educations.Select(c =>
           {
               var dto = _mapper.Map<ResultEducationLevelDto>(c);
               dto.Name = c.EducationLevelTranslations.FirstOrDefault(c => c.Language == lang)?.Name
                   ?? c.EducationLevelTranslations.FirstOrDefault(c => c.Language == "az")?.Name;

               return dto;
           });
            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetByIdEducationLevel(string lang, int id)
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

        [HttpDelete]
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