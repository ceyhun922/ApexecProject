using Microsoft.AspNetCore.Authorization;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.PlanningDTOs;
using ApexWebAPI.Entities;
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
        private readonly IStringLocalizer<PlanningsController> _localizer;

        public PlanningsController(ApexDbContext context, IStringLocalizer<PlanningsController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultPlanningDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultPlanningDto>>> GetAll([FromRoute] string lang)
        {
            var items = await _context.Plannings!
                .Include(p => p.Translations)
                .ToListAsync();

            var result = items.Select(item =>
            {
                var t = item.Translations?
                    .FirstOrDefault(x => x.Language == lang)
                    ?? item.Translations?.FirstOrDefault(x => x.Language == "az");

                return new ResultPlanningDto
                {
                    Id = item.Id,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    Badge = t?.Badge,
                    Title = t?.Title,
                    SubTitle = t?.SubTitle
                };
            }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromBody] CreatePlanningDto dto)
        {
            var existing = await _context.Plannings!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync();

            if (existing != null)
                _context.Plannings.Remove(existing);

            var item = new Planning
            {
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<PlanningTranslation>
                {
                    new() { Language = "az", Badge = dto.BadgeAz, Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Badge = dto.BadgeEn, Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Badge = dto.BadgeRu, Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Badge = dto.BadgeTr, Title = dto.TitleTr, SubTitle = dto.SubTitleTr }
                }
            };

            await _context.Plannings!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UpdatePlanningDto dto)
        {
            var item = await _context.Plannings!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.Status = dto.Status;

            var langs = new[]
            {
                ("az", dto.BadgeAz, dto.TitleAz, dto.SubTitleAz),
                ("en", dto.BadgeEn, dto.TitleEn, dto.SubTitleEn),
                ("ru", dto.BadgeRu, dto.TitleRu, dto.SubTitleRu),
                ("tr", dto.BadgeTr, dto.TitleTr, dto.SubTitleTr)
            };

            foreach (var (language, badge, title, subTitle) in langs)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Badge = badge;
                    t.Title = title;
                    t.SubTitle = subTitle;
                }
                else
                {
                    item.Translations!.Add(new PlanningTranslation
                    {
                        Language = language,
                        Badge = badge,
                        Title = title,
                        SubTitle = subTitle
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
