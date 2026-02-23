using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.TestimonialDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class TestimonialService : ITestimonialService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public TestimonialService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultTestimonialDto>> GetAllAsync(string lang)
        {
            var testimonials = await _context.Testimonials!.Include(t => t.Translations).ToListAsync();

            return testimonials.Select(t =>
            {
                var dto = _mapper.Map<ResultTestimonialDto>(t);
                dto.Comment = t.Translations.FirstOrDefault(tr => tr.Language == lang)?.Comment
                              ?? t.Translations.FirstOrDefault(tr => tr.Language == LanguageCodes.Az)?.Comment;
                return dto;
            });
        }

        public async Task<GetByIdTestimonialDto?> GetByIdAsync(string lang, int id)
        {
            var testimonial = await _context.Testimonials!
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (testimonial == null) return null;

            var dto = _mapper.Map<GetByIdTestimonialDto>(testimonial);
            dto.Comment = testimonial.Translations.FirstOrDefault(t => t.Language == lang)?.Comment
                          ?? testimonial.Translations.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Comment;
            return dto;
        }

        public async Task CreateAsync(CreateTestimonialDto dto)
        {
            var testimonial = _mapper.Map<Testimonial>(dto);
            testimonial.ImageUrl = dto.ImageUrl;
            testimonial.Translations = new List<TestimonialTranslation>
            {
                new() { Language = LanguageCodes.Az, Comment = dto.CommentAz },
                new() { Language = LanguageCodes.En, Comment = dto.CommentEn },
                new() { Language = LanguageCodes.Ru, Comment = dto.CommentRu },
                new() { Language = LanguageCodes.Tr, Comment = dto.CommentTr }
            };

            await _context.Testimonials!.AddAsync(testimonial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTestimonialDto dto)
        {
            var testimonial = await _context.Testimonials!
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == dto.Id)
                ?? throw new KeyNotFoundException($"Testimonial {dto.Id} not found");

            _mapper.Map(dto, testimonial);
            testimonial.ImageUrl = dto.ImageUrl;

            var translations = new Dictionary<string, string?>
            {
                { LanguageCodes.Az, dto.CommentAz },
                { LanguageCodes.En, dto.CommentEn },
                { LanguageCodes.Ru, dto.CommentRu },
                { LanguageCodes.Tr, dto.CommentTr }
            };

            foreach (var (language, comment) in translations)
            {
                var translation = testimonial.Translations.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                    translation.Comment = comment;
                else
                    testimonial.Translations.Add(new TestimonialTranslation { Language = language, Comment = comment });
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var testimonial = await _context.Testimonials!
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new KeyNotFoundException($"Testimonial {id} not found");

            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
        }
    }
}
