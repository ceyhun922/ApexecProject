using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.ContactHeaderDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class ContactHeadersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public ContactHeadersController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultContactHeaderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResultContactHeaderDto>> Get([FromRoute] string lang)
        {
            var header = await _context.ContactHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (header == null)
                return NotFound(new { message = "Contact header tapılmadı" });

            var translation = header.Translations?.FirstOrDefault(t => t.Language == lang);
            var dto = _mapper.Map<ResultContactHeaderDto>(header);
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateContactHeaderDto dto)
        {
            var existing = await _context.ContactHeaders!.Include(x => x.Translations).ToListAsync();
            foreach (var item in existing)
            {
                if (item.Translations != null)
                    _context.ContactHeaderTranslations!.RemoveRange(item.Translations);
            }
            _context.ContactHeaders!.RemoveRange(existing);

            var header = _mapper.Map<ContactHeader>(dto);
            header.Translations = new List<ContactHeaderTranslation>
            {
                new() { Language = "az", Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                new() { Language = "en", Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                new() { Language = "ru", Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                new() { Language = "tr", Title = dto.TitleTr, SubTitle = dto.SubTitleTr },
            };

            await _context.ContactHeaders.AddAsync(header);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Contact header yaradıldı" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateContactHeaderDto dto)
        {
            var header = await _context.ContactHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (header == null)
                return NotFound(new { message = "Contact header tapılmadı" });

            header.ImageUrl = dto.ImageUrl;
            header.Status = dto.Status;

            var translations = header.Translations ?? new List<ContactHeaderTranslation>();
            var langs = new[] { ("az", dto.TitleAz, dto.SubTitleAz), ("en", dto.TitleEn, dto.SubTitleEn),
                                ("ru", dto.TitleRu, dto.SubTitleRu), ("tr", dto.TitleTr, dto.SubTitleTr) };

            foreach (var (l, title, subtitle) in langs)
            {
                var t = translations.FirstOrDefault(x => x.Language == l);
                if (t != null) { t.Title = title; t.SubTitle = subtitle; }
                else header.Translations!.Add(new ContactHeaderTranslation { Language = l, Title = title, SubTitle = subtitle });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Contact header yeniləndi" });
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete()
        {
            var header = await _context.ContactHeaders!
                .Include(x => x.Translations)
                .FirstOrDefaultAsync();

            if (header == null)
                return NotFound(new { message = "Contact header tapılmadı" });

            if (header.Translations != null)
                _context.ContactHeaderTranslations!.RemoveRange(header.Translations);

            _context.ContactHeaders.Remove(header);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Contact header silindi" });
        }
    }
}
