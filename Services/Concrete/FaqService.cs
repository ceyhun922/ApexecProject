using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FaqDTO.cs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApexWebAPI.Common.HtmlSanitizerHelper;

namespace ApexWebAPI.Services.Concrete
{
    public class FaqService : IFaqService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public FaqService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultFaqDto>> GetAllAsync(string lang)
        {
            var faqs = await _context.Faqs!.Include(t => t.FaqTranslations).ToListAsync();

            return faqs.Select(f =>
            {
                var dto = _mapper.Map<ResultFaqDto>(f);
                var t = f.FaqTranslations?.FirstOrDefault(t => t.Language == lang)
                        ?? f.FaqTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az);
                dto.Title = t?.Title;
                dto.Content = t?.Content;
                return dto;
            });
        }

        public async Task<GetByIdFaqDto?> GetByIdAsync(string lang, int id)
        {
            var faq = await _context.Faqs!
                .Include(f => f.FaqTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (faq == null) return null;

            var dto = _mapper.Map<GetByIdFaqDto>(faq);
            var t = faq.FaqTranslations?.FirstOrDefault(t => t.Language == lang)
                    ?? faq.FaqTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az);
            dto.Title = t?.Title;
            dto.Content = t?.Content;
            return dto;
        }

        public async Task CreateAsync(CreateFaqDto dto)
        {
            var faq = _mapper.Map<Faq>(dto);
            faq.FaqTranslations = new List<FaqTranslation>
            {
                new() { Language = LanguageCodes.Az, Title = Sanitize(dto.TitleAz), Content = Sanitize(dto.ContentAz) },
                new() { Language = LanguageCodes.En, Title = Sanitize(dto.TitleEn), Content = Sanitize(dto.ContentEn) },
                new() { Language = LanguageCodes.Ru, Title = Sanitize(dto.TitleRu), Content = Sanitize(dto.ContentRu) },
                new() { Language = LanguageCodes.Tr, Title = Sanitize(dto.TitleTr), Content = Sanitize(dto.ContentTr) }
            };

            await _context.Faqs!.AddAsync(faq);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateFaqDto dto)
        {
            var faq = await _context.Faqs!
                .Include(f => f.FaqTranslations)
                .FirstOrDefaultAsync(f => f.Id == dto.Id)
                ?? throw new KeyNotFoundException($"FAQ {dto.Id} not found");

            _mapper.Map(dto, faq);

            var translations = new Dictionary<string, (string? Title, string? Content)>
            {
                { LanguageCodes.Az, (Sanitize(dto.TitleAz), Sanitize(dto.ContentAz)) },
                { LanguageCodes.En, (Sanitize(dto.TitleEn), Sanitize(dto.ContentEn)) },
                { LanguageCodes.Ru, (Sanitize(dto.TitleRu), Sanitize(dto.ContentRu)) },
                { LanguageCodes.Tr, (Sanitize(dto.TitleTr), Sanitize(dto.ContentTr)) }
            };

            foreach (var (language, (title, content)) in translations)
            {
                var translation = faq.FaqTranslations?.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = title;
                    translation.Content = content;
                }
                else
                {
                    faq.FaqTranslations ??= new List<FaqTranslation>();
                    faq.FaqTranslations.Add(new FaqTranslation { Language = language, Title = title, Content = content });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var faq = await _context.Faqs!
                .Include(f => f.FaqTranslations)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new KeyNotFoundException($"FAQ {id} not found");

            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();
        }
    }
}
