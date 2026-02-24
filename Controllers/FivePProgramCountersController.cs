using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FivePProgramCounterDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class FivePProgramCountersController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<FivePProgramCountersController> _localizer;

        public FivePProgramCountersController(ApexDbContext context, IMapper mapper, IStringLocalizer<FivePProgramCountersController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetByIdFivePProgramCounterDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdFivePProgramCounterDto>> Get([FromRoute] string lang)
        {
            var item = await _context.FivePProgramCounters!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdFivePProgramCounterDto>(item);

            var translation = item.Translations!
                .FirstOrDefault(t => t.Language == lang)
                ?? item.Translations!.FirstOrDefault(t => t.Language == "az");

            dto.Text1 = translation?.Text1;
            dto.Text2 = translation?.Text2;
            dto.Text3 = translation?.Text3;
            dto.Text4 = translation?.Text4;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create([FromRoute] string lang, [FromBody] CreateFivePProgramCounterDto dto)
        {
            var existing = await _context.FivePProgramCounters!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync();

            if (existing != null)
                _context.FivePProgramCounters.Remove(existing);

            var item = new FivePProgramCounter
            {
                Count1 = dto.Count1,
                Count2 = dto.Count2,
                Count3 = dto.Count3,
                Count4 = dto.Count4,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow,
                Translations = new List<FivePProgramCounterTranslation>
                {
                    new() { Language = "az", Text1 = dto.Text1Az, Text2 = dto.Text2Az, Text3 = dto.Text3Az, Text4 = dto.Text4Az },
                    new() { Language = "en", Text1 = dto.Text1En, Text2 = dto.Text2En, Text3 = dto.Text3En, Text4 = dto.Text4En },
                    new() { Language = "ru", Text1 = dto.Text1Ru, Text2 = dto.Text2Ru, Text3 = dto.Text3Ru, Text4 = dto.Text4Ru },
                    new() { Language = "tr", Text1 = dto.Text1Tr, Text2 = dto.Text2Tr, Text3 = dto.Text3Tr, Text4 = dto.Text4Tr },
                }
            };

            await _context.FivePProgramCounters!.AddAsync(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value, id = item.Id });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string lang, [FromBody] UpdateFivePProgramCounterDto dto)
        {
            var item = await _context.FivePProgramCounters!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            item.Count1 = dto.Count1;
            item.Count2 = dto.Count2;
            item.Count3 = dto.Count3;
            item.Count4 = dto.Count4;
            item.Status = dto.Status;

            var translations = new Dictionary<string, (string? T1, string? T2, string? T3, string? T4)>
            {
                { "az", (dto.Text1Az, dto.Text2Az, dto.Text3Az, dto.Text4Az) },
                { "en", (dto.Text1En, dto.Text2En, dto.Text3En, dto.Text4En) },
                { "ru", (dto.Text1Ru, dto.Text2Ru, dto.Text3Ru, dto.Text4Ru) },
                { "tr", (dto.Text1Tr, dto.Text2Tr, dto.Text3Tr, dto.Text4Tr) }
            };

            foreach (var (language, (t1, t2, t3, t4)) in translations)
            {
                var t = item.Translations!.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Text1 = t1; t.Text2 = t2; t.Text3 = t3; t.Text4 = t4;
                }
                else
                {
                    item.Translations!.Add(new FivePProgramCounterTranslation
                    {
                        Language = language,
                        Text1 = t1, Text2 = t2, Text3 = t3, Text4 = t4
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
            var item = await _context.FivePProgramCounters!
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.FivePProgramCounters.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}
