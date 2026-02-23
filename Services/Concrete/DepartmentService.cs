using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultDepartmentDto>> GetAllAsync(string lang)
        {
            var departments = await _context.Departments!.Include(d => d.DepartmentTranslations).ToListAsync();

            return departments.Select(d =>
            {
                var dto = _mapper.Map<ResultDepartmentDto>(d);
                dto.Name = d.DepartmentTranslations?.FirstOrDefault(t => t.Language == lang)?.Name
                           ?? d.DepartmentTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Name;
                return dto;
            });
        }

        public async Task<GetByIdDepartmentDto?> GetByIdAsync(string lang, int id)
        {
            var department = await _context.Departments!
                .Include(d => d.DepartmentTranslations)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return null;

            var dto = _mapper.Map<GetByIdDepartmentDto>(department);
            dto.Name = department.DepartmentTranslations?.FirstOrDefault(t => t.Language == lang)?.Name
                       ?? department.DepartmentTranslations?.FirstOrDefault(t => t.Language == LanguageCodes.Az)?.Name;
            return dto;
        }

        public async Task CreateAsync(CreateDepartmentDto dto)
        {
            var department = _mapper.Map<Department>(dto);
            department.DepartmentTranslations = new List<DepartmentTranslation>
            {
                new() { Language = LanguageCodes.Az, Name = dto.NameAz },
                new() { Language = LanguageCodes.En, Name = dto.NameEn },
                new() { Language = LanguageCodes.Tr, Name = dto.NameTr },
                new() { Language = LanguageCodes.Ru, Name = dto.NameRu }
            };

            await _context.Departments!.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateDepartmentDto dto)
        {
            var department = await _context.Departments!
                .Include(c => c.DepartmentTranslations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id)
                ?? throw new KeyNotFoundException($"Department {dto.Id} not found");

            _mapper.Map(dto, department);

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

                var translation = department.DepartmentTranslations?.FirstOrDefault(t => t.Language == language);
                if (translation == null)
                {
                    department.DepartmentTranslations ??= new List<DepartmentTranslation>();
                    department.DepartmentTranslations.Add(new DepartmentTranslation
                    {
                        DepartmentId = department.Id,
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
            var department = await _context.Departments!
                .Include(d => d.DepartmentTranslations)
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Department {id} not found");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
