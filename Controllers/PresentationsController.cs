using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.PresentationDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class PresentationsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<PresentationsController> _localizer;

        public PresentationsController(ApexDbContext context, IMapper mapper, IStringLocalizer<PresentationsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResultPresentationDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultPresentationDto>>> GetAll([FromRoute] string lang)
        {
            var presentations = await _context.Presentations!
                .Include(p => p.Translations)
                .Where(p => p.Status)
                .ToListAsync();

            var result = presentations.Select(p =>
            {
                var dto = _mapper.Map<ResultPresentationDto>(p);

                var translation = p.Translations!
                    .FirstOrDefault(t => t.Language == lang)
                    ?? p.Translations!.FirstOrDefault(t => t.Language == "az");

                dto.Title = translation?.Title;
                dto.SubTitle = translation?.SubTitle;

                return dto;
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdPresentationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdPresentationDto>> GetById([FromRoute] string lang, int id)
        {
            var presentation = await _context.Presentations!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (presentation == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdPresentationDto>(presentation);

            var translation = presentation.Translations!
                .FirstOrDefault(t => t.Language == lang)
                ?? presentation.Translations!.FirstOrDefault(t => t.Language == "az");

            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreatePresentationDto dto)
        {
            var presentation = new Presentation
            {
                YouTubeUrl = dto.YouTubeUrl,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<PresentationTranslation>
                {
                    new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                    new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                    new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                    new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
                }
            };

            await _context.Presentations!.AddAsync(presentation);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = presentation.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdatePresentationDto dto)
        {
            var presentation = await _context.Presentations!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (presentation == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            presentation.YouTubeUrl = dto.YouTubeUrl;
            presentation.Status = dto.Status;

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { "az", (dto.TitleAz, dto.SubTitleAz) },
                { "en", (dto.TitleEn, dto.SubTitleEn) },
                { "ru", (dto.TitleRu, dto.SubTitleRu) },
                { "tr", (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var t = presentation.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Title = title;
                    t.SubTitle = subTitle;
                }
                else
                {
                    presentation.Translations!.Add(new PresentationTranslation
                    {
                        Language = language,
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
            var presentation = await _context.Presentations!
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (presentation == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Presentations.Remove(presentation);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
