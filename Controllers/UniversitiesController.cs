using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.UniversityDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class UniversitiesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UniversitiesController> _localizer;

        public UniversitiesController(ApexDbContext context, IMapper mapper, IStringLocalizer<UniversitiesController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultUniversityDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultUniversityDto>>> GetAll(
            [FromRoute] string lang,
            [FromQuery] int? countryId,
            [FromQuery] int? educationLevelId,
            [FromQuery] int? departmentId)
        {
            var query = _context.Universities!
                .Include(u => u.Translations)
                .Where(u => u.Status)
                .AsQueryable();

            if (countryId.HasValue)
                query = query.Where(u => u.CountryId == countryId.Value);

            if (educationLevelId.HasValue)
                query = query.Where(u => u.EducationLevelId == educationLevelId.Value);

            if (departmentId.HasValue)
                query = query.Where(u => u.DepartmentId == departmentId.Value);

            var items = await query.ToListAsync();

            var result = items.Select(u =>
            {
                var dto = _mapper.Map<ResultUniversityDto>(u);
                var t = u.Translations.FirstOrDefault(t => t.Language == lang)
                    ?? u.Translations.FirstOrDefault(t => t.Language == "az");
                dto.Title = t?.Title;
                dto.SubTitle = t?.SubTitle;
                dto.Description = t?.Description;
                return dto;
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdUniversityDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdUniversityDto>> GetById([FromRoute] string lang, int id)
        {
            var item = await _context.Universities!
                .Include(u => u.Translations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdUniversityDto>(item);
            var t = item.Translations.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations.FirstOrDefault(t => t.Language == "az");
            dto.Title = t?.Title;
            dto.SubTitle = t?.SubTitle;
            dto.Description = t?.Description;

            return Ok(dto);
        }

        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdUniversityDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdUniversityDto>> GetDetail([FromRoute] string lang, int id)
        {
            var item = await _context.Universities!
                .Include(u => u.Translations)
                .FirstOrDefaultAsync(u => u.Id == id && u.Status);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdUniversityDto>(item);
            var t = item.Translations.FirstOrDefault(t => t.Language == lang)
                ?? item.Translations.FirstOrDefault(t => t.Language == "az");
            dto.Title = t?.Title;
            dto.SubTitle = t?.SubTitle;
            dto.Description = t?.Description;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreateUniversityDto dto)
        {
            var item = new University
            {
                ImageUrl = dto.ImageUrl,
                Status = dto.Status,
                CountryId = dto.CountryId,
                EducationLevelId = dto.EducationLevelId,
                DepartmentId = dto.DepartmentId,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<UniversityTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz, Description = dto.DescriptionAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn, Description = dto.DescriptionEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu, Description = dto.DescriptionRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr, Description = dto.DescriptionTr }
                }
            };

            await _context.Universities!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdateUniversityDto dto)
        {
            var item = await _context.Universities!
                .Include(u => u.Translations)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.ImageUrl = dto.ImageUrl;
            item.Status = dto.Status;
            item.CountryId = dto.CountryId;
            item.EducationLevelId = dto.EducationLevelId;
            item.DepartmentId = dto.DepartmentId;

            var langs = new[]
            {
                ("az", dto.TitleAz, dto.SubTitleAz, dto.DescriptionAz),
                ("en", dto.TitleEn, dto.SubTitleEn, dto.DescriptionEn),
                ("ru", dto.TitleRu, dto.SubTitleRu, dto.DescriptionRu),
                ("tr", dto.TitleTr, dto.SubTitleTr, dto.DescriptionTr)
            };

            foreach (var (l, title, subTitle, description) in langs)
            {
                var t = item.Translations.FirstOrDefault(x => x.Language == l);
                if (t != null) { t.Title = title; t.SubTitle = subTitle; t.Description = description; }
                else item.Translations.Add(new UniversityTranslation { Language = l, Title = title, SubTitle = subTitle, Description = description });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Universities!
                .Include(u => u.Translations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.UniversityTranslations!.RemoveRange(item.Translations);
            _context.Universities.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
