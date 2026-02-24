using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.SummerSchoolDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApexWebAPI.Common.HtmlSanitizerHelper;

namespace ApexWebAPI.Services.Concrete
{
    public class SummerSchoolService : ISummerSchoolService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public SummerSchoolService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultSummerSchoolDto>> GetAllAsync(string lang)
        {
            var schools = await _context.SummerSchools!
                .Include(s => s.Translations)
                .Where(s => s.Status)
                .ToListAsync();

            return schools.Select(s =>
            {
                var dto = _mapper.Map<ResultSummerSchoolDto>(s);
                var t = s.Translations.FirstOrDefault(t => t.Language == lang)
                        ?? s.Translations.FirstOrDefault(t => t.Language == LanguageCodes.Az);
                dto.Title = t?.Title;
                dto.SubTitle = t?.SubTitle;
                return dto;
            });
        }

        public async Task CreateAsync(CreateSummerSchoolDto dto)
        {
            var school = _mapper.Map<SummerSchool>(dto);
            school.CountryId = dto.CountryId;
            school.ImageUrl = dto.ImageUrl;
            school.Translations = new List<SummerSchoolTranslation>
            {
                new() { Language = LanguageCodes.Az, Title = Sanitize(dto.TitleAz), SubTitle = Sanitize(dto.SubTitleAz) },
                new() { Language = LanguageCodes.En, Title = Sanitize(dto.TitleEn), SubTitle = Sanitize(dto.SubTitleEn) },
                new() { Language = LanguageCodes.Ru, Title = Sanitize(dto.TitleRu), SubTitle = Sanitize(dto.SubTitleRu) },
                new() { Language = LanguageCodes.Tr, Title = Sanitize(dto.TitleTr), SubTitle = Sanitize(dto.SubTitleTr) }
            };

            await _context.SummerSchools!.AddAsync(school);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateSummerSchoolDto dto)
        {
            var school = await _context.SummerSchools!
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == dto.Id)
                ?? throw new KeyNotFoundException($"SummerSchool {dto.Id} not found");

            school.ImageUrl = dto.ImageUrl;
            school.Status = dto.Status;

            var translations = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { LanguageCodes.Az, (Sanitize(dto.TitleAz), Sanitize(dto.SubTitleAz)) },
                { LanguageCodes.En, (Sanitize(dto.TitleEn), Sanitize(dto.SubTitleEn)) },
                { LanguageCodes.Ru, (Sanitize(dto.TitleRu), Sanitize(dto.SubTitleRu)) },
                { LanguageCodes.Tr, (Sanitize(dto.TitleTr), Sanitize(dto.SubTitleTr)) }
            };

            foreach (var (language, value) in translations)
            {
                var translation = school.Translations.FirstOrDefault(t => t.Language == language);
                if (translation != null)
                {
                    translation.Title = value.Title;
                    translation.SubTitle = value.SubTitle;
                }
                else
                {
                    school.Translations.Add(new SummerSchoolTranslation
                    {
                        Language = language,
                        Title = value.Title,
                        SubTitle = value.SubTitle
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
