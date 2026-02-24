using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.PlanningDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class PlanningsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<PlanningsController> _localizer;

        public PlanningsController(ApexDbContext context, IMapper mapper, IStringLocalizer<PlanningsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultPlanningDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultPlanningDto>> Get([FromRoute] string lang)
        {
            var item = await _context.Plannings!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<ResultPlanningDto>(item);

            var translation = item.Translations!
                .FirstOrDefault(t => t.Language == lang)
                ?? item.Translations!.FirstOrDefault(t => t.Language == "az");

            dto.Option1Title = translation?.Option1Title;
            dto.Option2Title = translation?.Option2Title;
            dto.Option3Title = translation?.Option3Title;
            dto.Option4Title = translation?.Option4Title;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreatePlanningDto dto)
        {
            var existing = await _context.Plannings!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync();

            if (existing != null)
                _context.Plannings.Remove(existing);

            var item = new Planning
            {
                Option1 = dto.Option1,
                Option2 = dto.Option2,
                Option3 = dto.Option3,
                Option4 = dto.Option4,
                Checkbox1 = dto.Checkbox1,
                Checkbox2 = dto.Checkbox2,
                Checkbox3 = dto.Checkbox3,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<PlanningTranslation>
                {
                    new() { Language = "az", Option1Title = dto.Option1TitleAz, Option2Title = dto.Option2TitleAz, Option3Title = dto.Option3TitleAz, Option4Title = dto.Option4TitleAz },
                    new() { Language = "en", Option1Title = dto.Option1TitleEn, Option2Title = dto.Option2TitleEn, Option3Title = dto.Option3TitleEn, Option4Title = dto.Option4TitleEn },
                    new() { Language = "ru", Option1Title = dto.Option1TitleRu, Option2Title = dto.Option2TitleRu, Option3Title = dto.Option3TitleRu, Option4Title = dto.Option4TitleRu },
                    new() { Language = "tr", Option1Title = dto.Option1TitleTr, Option2Title = dto.Option2TitleTr, Option3Title = dto.Option3TitleTr, Option4Title = dto.Option4TitleTr },
                }
            };

            await _context.Plannings!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdatePlanningDto dto)
        {
            var item = await _context.Plannings!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.Option1 = dto.Option1;
            item.Option2 = dto.Option2;
            item.Option3 = dto.Option3;
            item.Option4 = dto.Option4;
            item.Checkbox1 = dto.Checkbox1;
            item.Checkbox2 = dto.Checkbox2;
            item.Checkbox3 = dto.Checkbox3;
            item.Status = dto.Status;

            var translations = new Dictionary<string, (string? T1, string? T2, string? T3, string? T4)>
            {
                { "az", (dto.Option1TitleAz, dto.Option2TitleAz, dto.Option3TitleAz, dto.Option4TitleAz) },
                { "en", (dto.Option1TitleEn, dto.Option2TitleEn, dto.Option3TitleEn, dto.Option4TitleEn) },
                { "ru", (dto.Option1TitleRu, dto.Option2TitleRu, dto.Option3TitleRu, dto.Option4TitleRu) },
                { "tr", (dto.Option1TitleTr, dto.Option2TitleTr, dto.Option3TitleTr, dto.Option4TitleTr) }
            };

            foreach (var (language, (t1, t2, t3, t4)) in translations)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Option1Title = t1; t.Option2Title = t2; t.Option3Title = t3; t.Option4Title = t4;
                }
                else
                {
                    item.Translations!.Add(new PlanningTranslation
                    {
                        Language = language,
                        Option1Title = t1, Option2Title = t2, Option3Title = t3, Option4Title = t4
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
            var item = await _context.Plannings!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Plannings.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
