using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FeatureDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class HeroService : IHeroService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public HeroService(ApexDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<ResultHeroDto>> GetAllAsync(string lang)
        {
            var heroes = await _context.Heroes!
                .Include(h => h.Translations)
                .Where(h => h.Status)
                .ToListAsync();

            return heroes.Select(h =>
            {
                var dto = _mapper.Map<ResultHeroDto>(h);
                var translation = h.Translations?.FirstOrDefault(t => t.Language == lang)
                                  ?? h.Translations?.FirstOrDefault(t => t.Language == LanguageCodes.Az);
                dto.Title = translation?.Title;
                dto.SubTitle = translation?.SubTitle;
                return dto;
            });
        }

        public async Task<GetByIdHeroDto?> GetByIdAsync(string lang, int id)
        {
            var hero = await _context.Heroes!
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hero == null) return null;

            var dto = _mapper.Map<GetByIdHeroDto>(hero);
            var translation = hero.Translations?.FirstOrDefault(t => t.Language == lang)
                              ?? hero.Translations?.FirstOrDefault(t => t.Language == LanguageCodes.Az);
            dto.Title = translation?.Title;
            dto.SubTitle = translation?.SubTitle;
            return dto;
        }

        public async Task<int> CreateAsync(string lang, CreateHeroDto dto)
        {
            string? videoUrl = null;

            if (dto.Video != null && dto.Video.Length > 0)
                videoUrl = await SaveFileAsync(dto.Video, "videos");

            var hero = new Hero
            {
                VideoUrl = videoUrl,
                Status = true,
                CreatedDate = DateTime.UtcNow,
                Translations = LanguageCodes.All.Select(l => new HeroTranslation
                {
                    Language = l,
                    Title = l == LanguageCodes.Az ? dto.TitleAz
                          : l == LanguageCodes.En ? dto.TitleEn
                          : l == LanguageCodes.Ru ? dto.TitleRu
                          : dto.TitleTr,
                    SubTitle = l == LanguageCodes.Az ? dto.SubTitleAz
                             : l == LanguageCodes.En ? dto.SubTitleEn
                             : l == LanguageCodes.Ru ? dto.SubTitleRu
                             : dto.SubTitleTr
                }).ToList()
            };

            await _context.Heroes!.AddAsync(hero);
            await _context.SaveChangesAsync();
            return hero.Id;
        }

        public async Task UpdateAsync(string lang, UpdateHeroDto dto)
        {
            var hero = await _context.Heroes!
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == dto.Id)
                ?? throw new KeyNotFoundException($"Hero {dto.Id} not found");

            if (dto.Video != null && dto.Video.Length > 0)
            {
                DeleteFileIfExists(hero.VideoUrl);
                hero.VideoUrl = await SaveFileAsync(dto.Video, "videos");
            }

            hero.Status = dto.Status;

            var translationMap = new Dictionary<string, (string? Title, string? SubTitle)>
            {
                { LanguageCodes.Az, (dto.TitleAz, dto.SubTitleAz) },
                { LanguageCodes.En, (dto.TitleEn, dto.SubTitleEn) },
                { LanguageCodes.Ru, (dto.TitleRu, dto.SubTitleRu) },
                { LanguageCodes.Tr, (dto.TitleTr, dto.SubTitleTr) }
            };

            foreach (var (language, (title, subTitle)) in translationMap)
            {
                var t = hero.Translations?.FirstOrDefault(x => x.Language == language);
                if (t != null)
                {
                    t.Title = title;
                    t.SubTitle = subTitle;
                }
                else
                {
                    hero.Translations ??= new List<HeroTranslation>();
                    hero.Translations.Add(new HeroTranslation { Language = language, Title = title, SubTitle = subTitle });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hero = await _context.Heroes!
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == id)
                ?? throw new KeyNotFoundException($"Hero {id} not found");

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var folder = Path.Combine(webRoot, subfolder);
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{subfolder}/{fileName}";
        }

        private void DeleteFileIfExists(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var fullPath = Path.Combine(webRoot, relativePath.TrimStart('/'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
