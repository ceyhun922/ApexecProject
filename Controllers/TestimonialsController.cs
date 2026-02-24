using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.TestimonialDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class TestimonialsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<TestimonialsController> _localizer;

        public TestimonialsController(ApexDbContext context, IMapper mapper, IStringLocalizer<TestimonialsController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ResultTestimonialDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResultTestimonialDto>>> GetALL(string lang)
        {
            var testimonials = await _context.Testimonials
                .Include(t => t.Translations).ToListAsync();

            var result = testimonials.Select(t =>
            {
                var dto = _mapper.Map<ResultTestimonialDto>(t);
                dto.Comment = t.Translations.FirstOrDefault(t => t.Language == lang)?.Comment
                    ?? t.Translations.FirstOrDefault(t => t.Language == "az")?.Comment;

                return dto;
            });

            return Ok(result);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetByIdTestimonialDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdTestimonialDto>> GetById(string lang, int id)
        {
            var feature = await _context.Testimonials
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feature == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdTestimonialDto>(feature);
            dto.Comment = feature.Translations.FirstOrDefault(t => t.Language == lang)?.Comment
                        ?? feature.Translations.FirstOrDefault(t => t.Language == "az")?.Comment;

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateTestimonialDto dto)
        {
            var testimonial = _mapper.Map<Testimonial>(dto);

            testimonial.ImageUrl = dto.ImageUrl;
            testimonial.Translations = new List<TestimonialTranslation>
            {
                new() { Language = "az", Comment = dto.CommentAz},
                new() { Language = "en", Comment = dto.CommentEn},
                new() { Language = "ru", Comment = dto.CommentRu},
                new() { Language = "tr", Comment = dto.CommentTr},
            };

            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = _localizer["Created"].Value });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateTestimonialDto dto)
        {
            var testimonial = await _context.Testimonials
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (testimonial == null)
                return NotFound(new { message = _localizer["NotFount"].Value });

            _mapper.Map(dto, testimonial);

            var translations = new Dictionary<string, string?>
                {
                    { "az", dto.CommentAz },
                    { "en", dto.CommentEn },
                    { "ru", dto.CommentRu },
                    { "tr", dto.CommentTr }
                };

            foreach (var (language, comment) in translations)
            {
                var translation = testimonial.Translations.FirstOrDefault(t => t.Language == language);

                if (translation != null)
                {
                    translation.Comment = comment;
                }
                else
                {
                    testimonial.Translations.Add(new TestimonialTranslation
                    {
                        Language = language,
                        Comment = comment
                    });
                }
            }

            testimonial.ImageUrl = dto.ImageUrl;
            await _context.SaveChangesAsync();


            return Ok(new { message = _localizer["Updated"].Value });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var testimonial = await _context.Testimonials
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (testimonial == null)
                return NotFound(new { message = _localizer["NotFount"].Value });

            _context.Testimonials.Remove(testimonial);

            foreach (var translation in testimonial.Translations)
            {
                _context.Translations.Remove(translation);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }

    }
}