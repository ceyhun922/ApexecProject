using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FaqDTO.cs;
using Microsoft.AspNetCore.Mvc;
using ApexWebAPI.DTOs.TestimonialDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ApexWebAPI.DTOs.FeatureDTOs;

namespace ApexWebAPI.Controllers
{
     [ApiController]
    [Route("api/{lang}/[controller]")]
    public class FaqsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<FaqsController> _localizer;

        public FaqsController(ApexDbContext context, IMapper mapper, IStringLocalizer<FaqsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL(string lang)
        {
            var testimonials = await _context.Faqs
                .Include(t => t.FaqTranslations).ToListAsync();

            var result = testimonials.Select(t =>
            {
                var dto = _mapper.Map<ResultFaqDto>(t);
                dto.Title = t.FaqTranslations.FirstOrDefault(t => t.Language == lang)?.Title
                    ?? t.FaqTranslations.FirstOrDefault(t => t.Language == "az")?.Title;

                dto.Content =t.FaqTranslations.FirstOrDefault(t=>t.Language ==lang)?.Content
                    ?? t.FaqTranslations.FirstOrDefault(t=>t.Language ==lang)?.Content;

                return dto;
            });

            return Ok(result);
        } 

         [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string lang, int id)
        {
            var feature = await _context.Faqs
                .Include(f => f.FaqTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feature == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdHeroDto>(feature);
            dto.Title = feature.FaqTranslations.FirstOrDefault(t => t.Language == lang)?.Title
                        ?? feature.FaqTranslations.FirstOrDefault(t => t.Language == "az")?.Title;
            dto.SubTitle = feature.FaqTranslations.FirstOrDefault(t => t.Language == lang)?.Content
                           ?? feature.FaqTranslations.FirstOrDefault(t => t.Language == "az")?.Content;

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateFaqDto dto)
        {
            var faq = _mapper.Map<Faq>(dto);

            faq.FaqTranslations = new List<FaqTranslation>
            {
                new FaqTranslation { Language = "az", Title = dto.TitleAz, Content = dto.ContentAz },
                new FaqTranslation { Language = "en", Title = dto.TitleEn, Content = dto.ContentEn },
                new FaqTranslation { Language = "ru", Title = dto.TitleRu, Content = dto.ContentRu },
                new FaqTranslation { Language = "tr", Title = dto.TitleTr, Content = dto.ContentTr }
            };

            await _context.Faqs.AddAsync(faq);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Created" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateFaqDto dto)
        {
            var feature = await _context.Faqs
                .Include(f => f.FaqTranslations)
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (feature == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _mapper.Map(dto, feature);

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { "az", (dto.TitleAz, dto.ContentAz) },
                { "en", (dto.TitleEn, dto.ContentEn) },
                { "ru", (dto.TitleRu, dto.ContentRu) },
                { "tr", (dto.TitleTr, dto.ContentTr) }
            };

            foreach (var (language, (title, content)) in translations)
            {
                var translation = feature.FaqTranslations.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = title;
                    translation.Content = content;
                }
                else
                {
                    feature.FaqTranslations.Add(new FaqTranslation
                    {
                        Language = language,
                        Title = title,
                         Content= content
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = _localizer["Updated"].Value });
        }

         [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var faq = await _context.Faqs
                .Include(f => f.FaqTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (faq == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }
    }
}