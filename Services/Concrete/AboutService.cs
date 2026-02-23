using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.AboutDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class AboutService : IAboutService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public AboutService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultAboutDto>> GetAllAsync(string lang)
        {
            var abouts = await _context.Abouts!.Include(x => x.AboutTranslations).ToListAsync();

            return abouts.Select(a =>
            {
                var dto = _mapper.Map<ResultAboutDto>(a);
                var t = a.AboutTranslations?.FirstOrDefault(t => t.Language == lang)
                        ?? a.AboutTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az);
                dto.Title = t?.Title;
                dto.SubTitle = t?.SubTitle;
                return dto;
            });
        }

        public async Task<GetByIdAboutDto?> GetByIdAsync(string lang, int id)
        {
            var about = await _context.Abouts!
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (about == null) return null;

            var dto = _mapper.Map<GetByIdAboutDto>(about);
            var t = about.AboutTranslations?.FirstOrDefault(t => t.Language == lang)
                    ?? about.AboutTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az);
            dto.Title = t?.Title;
            dto.SubTitle = t?.SubTitle;
            return dto;
        }

        public async Task CreateAsync(CreateAboutDto dto)
        {
            var about = _mapper.Map<About>(dto);
            about.ImageUrl = dto.ImageUrl;
            about.AboutTranslations = new List<AboutTranslation>
            {
                new() { Language = LanguageCodes.Az, Title = dto.TitleAz, SubTitle = dto.SubTitleAz },
                new() { Language = LanguageCodes.En, Title = dto.TitleEn, SubTitle = dto.SubTitleEn },
                new() { Language = LanguageCodes.Ru, Title = dto.TitleRu, SubTitle = dto.SubTitleRu },
                new() { Language = LanguageCodes.Tr, Title = dto.TitleTr, SubTitle = dto.SubTitleTr }
            };

            await _context.Abouts!.AddAsync(about);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateAboutDto dto)
        {
            var about = await _context.Abouts!
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("About not found");

            _mapper.Map(dto, about);
            about.ImageUrl = dto.ImageUrl;

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { LanguageCodes.Az, (dto.TitleAz, dto.SubTitleAz) },
                { LanguageCodes.En, (dto.TitleEn, dto.SubTitleEn) },
                { LanguageCodes.Ru, (dto.TitleRu, dto.SubTitleRu) },
                { LanguageCodes.Tr, (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translations)
            {
                var translation = about.AboutTranslations?.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = title;
                    translation.SubTitle = subTitle;
                }
                else
                {
                    about.AboutTranslations ??= new List<AboutTranslation>();
                    about.AboutTranslations.Add(new AboutTranslation { Language = language, Title = title, SubTitle = subTitle });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var about = await _context.Abouts!
                .Include(f => f.AboutTranslations)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new KeyNotFoundException($"About {id} not found");

            _context.Abouts.Remove(about);
            await _context.SaveChangesAsync();
        }
    }
}
