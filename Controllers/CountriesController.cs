using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CountriesController> _localizer;

        public CountriesController(ApexDbContext context, IMapper mapper, IStringLocalizer<CountriesController> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string lang)
        {
            var countries = await _context.Countries.Include(c => c.CountryTranslations).ToListAsync();

            var result = countries.Select(c =>
            {
                var dto = _mapper.Map<ResultCountryDto>(c);
                dto.Name = c.CountryTranslations.FirstOrDefault(c => c.Language == lang)?.Name
                    ?? c.CountryTranslations.FirstOrDefault(c => c.Language == "az")?.Name;

                return dto;
            });

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string lang, int id)
        {
            var country = await _context.Countries
                .Include(c => c.CountryTranslations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            var dto = _mapper.Map<GetByIdCountryDto>(country);
            dto.Name = country.CountryTranslations.FirstOrDefault(c => c.Language == lang)?.Name
                ?? country.CountryTranslations.FirstOrDefault(c => c.Language == "az")?.Name;

            return Ok(dto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _context.Countries
                .Include(f => f.CountryTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (country == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return Ok(new { message = _localizer["Deleted"].Value });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCountryDto dto)
        {
            var countries = _mapper.Map<Country>(dto);

            countries.CountryTranslations = new List<CountryTranslation>
            {
                new CountryTranslation { Language ="az", Name =dto.NameAz},
                new CountryTranslation { Language ="en", Name =dto.NameEn},
                new CountryTranslation { Language ="tr", Name =dto.NameTr},
                new CountryTranslation { Language ="ru", Name =dto.NameRu}
            };

            await _context.AddAsync(countries);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Created" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCountryDto dto)
        {
            var country = await _context.Countries
                .Include(c => c.CountryTranslations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (country == null)
                return NotFound(new { message = _localizer["NotFound"].Value });

            _mapper.Map(dto, country);

            var translations = new Dictionary<string, string?>
            {
                ["az"] = dto.NameAz,
                ["en"] = dto.NameEn,
                ["tr"] = dto.NameTr,
                ["ru"] = dto.NameRu
            };

            foreach (var (language, name) in translations)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                var translation = country.CountryTranslations
                    .FirstOrDefault(t => t.Language == language);

                if (translation == null)
                {
                    country.CountryTranslations.Add(new CountryTranslation
                    {
                        CountryId = country.Id,
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

            return Ok(new { message = _localizer["Updated"].Value });
        }

    }
}