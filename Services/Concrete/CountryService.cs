using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class CountryService : ICountryService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public CountryService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultCountryDto>> GetAllAsync(string lang)
        {
            var countries = await _context.Countries!.Include(c => c.CountryTranslations).ToListAsync();

            return countries.Select(c =>
            {
                var dto = _mapper.Map<ResultCountryDto>(c);
                dto.Name = c.CountryTranslations?.FirstOrDefault(t => t.Language == lang)?.Name
                           ?? c.CountryTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Name;
                return dto;
            });
        }

        public async Task<GetByIdCountryDto?> GetByIdAsync(string lang, int id)
        {
            var country = await _context.Countries!
                .Include(c => c.CountryTranslations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null) return null;

            var dto = _mapper.Map<GetByIdCountryDto>(country);
            dto.Name = country.CountryTranslations?.FirstOrDefault(t => t.Language == lang)?.Name
                       ?? country.CountryTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Name;
            return dto;
        }

        public async Task CreateAsync(CreateCountryDto dto)
        {
            var country = _mapper.Map<Country>(dto);
            country.CountryTranslations = new List<CountryTranslation>
            {
                new() { Language = LanguageCodes.Az, Name = dto.NameAz },
                new() { Language = LanguageCodes.En, Name = dto.NameEn },
                new() { Language = LanguageCodes.Tr, Name = dto.NameTr },
                new() { Language = LanguageCodes.Ru, Name = dto.NameRu }
            };

            await _context.Countries!.AddAsync(country);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateCountryDto dto)
        {
            var country = await _context.Countries!
                .Include(c => c.CountryTranslations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id)
                ?? throw new KeyNotFoundException($"Country {dto.Id} not found");

            _mapper.Map(dto, country);

            var translations = new Dictionary<string, string?>
            {
                [LanguageCodes.Az] = dto.NameAz,
                [LanguageCodes.En] = dto.NameEn,
                [LanguageCodes.Tr] = dto.NameTr,
                [LanguageCodes.Ru] = dto.NameRu
            };

            foreach (var (language, name) in translations)
            {
                if (string.IsNullOrWhiteSpace(name)) continue;

                var translation = country.CountryTranslations?.FirstOrDefault(t => t.Language == language);
                if (translation == null)
                {
                    country.CountryTranslations ??= new List<CountryTranslation>();
                    country.CountryTranslations.Add(new CountryTranslation { CountryId = country.Id, Language = language, Name = name });
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
            var country = await _context.Countries!
                .Include(f => f.CountryTranslations)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new KeyNotFoundException($"Country {id} not found");

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }
    }
}
