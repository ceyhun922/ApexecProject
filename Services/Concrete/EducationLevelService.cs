using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApexWebAPI.Common.HtmlSanitizerHelper;

namespace ApexWebAPI.Services.Concrete
{
    public class EducationLevelService : IEducationLevelService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public EducationLevelService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultEducationLevelDto>> GetAllAsync(string lang)
        {
            var levels = await _context.EducationLevels!.Include(el => el.EducationLevelTranslations).ToListAsync();

            return levels.Select(el =>
            {
                var dto = _mapper.Map<ResultEducationLevelDto>(el);
                dto.Name = el.EducationLevelTranslations.FirstOrDefault(t => t.Language == lang)?.Name
                           ?? el.EducationLevelTranslations.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Name;
                return dto;
            });
        }

        public async Task<GetByIdEducationLevelDto?> GetByIdAsync(string lang, int id)
        {
            var level = await _context.EducationLevels!
                .Include(ed => ed.EducationLevelTranslations)
                .FirstOrDefaultAsync(ed => ed.Id == id);

            if (level == null) return null;

            var dto = _mapper.Map<GetByIdEducationLevelDto>(level);
            dto.Name = level.EducationLevelTranslations.FirstOrDefault(t => t.Language == lang)?.Name
                       ?? level.EducationLevelTranslations.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Name;
            return dto;
        }

        public async Task CreateAsync(CreateEducationLevelDto dto)
        {
            var level = _mapper.Map<EducationLevel>(dto);
            level.EducationLevelTranslations = new List<EducationLevelTranslation>
            {
                new() { Language = LanguageCodes.Az, Name = Sanitize(dto.NameAz) },
                new() { Language = LanguageCodes.En, Name = Sanitize(dto.NameEn) },
                new() { Language = LanguageCodes.Tr, Name = Sanitize(dto.NameTr) },
                new() { Language = LanguageCodes.Ru, Name = Sanitize(dto.NameRu) }
            };

            await _context.EducationLevels!.AddAsync(level);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateEducationLevelDto dto)
        {
            var level = await _context.EducationLevels!
                .Include(ed => ed.EducationLevelTranslations)
                .FirstOrDefaultAsync(ed => ed.Id == dto.Id)
                ?? throw new KeyNotFoundException($"EducationLevel {dto.Id} not found");

            _mapper.Map(dto, level);

            var translations = new Dictionary<string, string>
            {
                [LanguageCodes.Az] = Sanitize(dto.NameAz) ?? string.Empty,
                [LanguageCodes.En] = Sanitize(dto.NameEn) ?? string.Empty,
                [LanguageCodes.Ru] = Sanitize(dto.NameRu) ?? string.Empty,
                [LanguageCodes.Tr] = Sanitize(dto.NameTr) ?? string.Empty
            };

            foreach (var (language, name) in translations)
            {
                var translation = level.EducationLevelTranslations.FirstOrDefault(t => t.Language == language);
                if (translation == null)
                {
                    level.EducationLevelTranslations.Add(new EducationLevelTranslation
                    {
                        EducationLevelId = level.Id,
                        Language = language,
                        Name = name
                    });
                }
                else
                {
                    translation.Name = name;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var level = await _context.EducationLevels!
                .Include(ed => ed.EducationLevelTranslations)
                .FirstOrDefaultAsync(ed => ed.Id == id)
                ?? throw new KeyNotFoundException($"EducationLevel {id} not found");

            _context.EducationLevels.Remove(level);
            await _context.SaveChangesAsync();
        }
    }
}
