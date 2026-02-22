using System.Threading.Tasks;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentsController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string lang)
        {
            var departments = await _context.Departments.Include(d => d.DepartmentTranslations).ToListAsync();

            var result = departments.Select(d =>
              {
                  var dto = _mapper.Map<ResultDepartmentDto>(d);
                  dto.Name = d.DepartmentTranslations.FirstOrDefault(c => c.Language == lang)?.Name
                      ?? d.DepartmentTranslations.FirstOrDefault(c => c.Language == "az")?.Name;

                  return dto;
              });

            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetByIdDepartmentLevel(string lang, int id)
        {
            var education = await _context.Departments.Include(ed => ed.DepartmentTranslations).FirstOrDefaultAsync(ed => ed.Id == id);
            if (education == null) return NotFound();
            var dto = _mapper.Map<GetByIdDepartmentDto>(education);
            dto.Name = education.DepartmentTranslations.FirstOrDefault(ed => ed.Language == lang)?.Name
                ?? education.DepartmentTranslations.FirstOrDefault(ed => ed.Language == "az")?.Name;
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            var educations = _mapper.Map<Department>(dto);

            educations.DepartmentTranslations = new List<DepartmentTranslation>
            {
                 new DepartmentTranslation { Language ="az", Name =dto.NameAz},
                new DepartmentTranslation { Language ="en", Name =dto.NameEn},
                new DepartmentTranslation { Language ="tr", Name =dto.NameTr},
                new DepartmentTranslation { Language ="ru", Name =dto.NameRu}
            };

            await _context.AddAsync(educations);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { message = "Created" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateDepartmentDto dto)
        {
            var department = await _context.Departments
                .Include(c => c.DepartmentTranslations)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (department == null)
                return NotFound();

            _mapper.Map(dto, department);

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

                var translation = department.DepartmentTranslations
                    .FirstOrDefault(t => t.Language == language);

                if (translation == null)
                {
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

            return Ok(new { message = "Updated" });
        }

          [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments
                .Include(d => d.DepartmentTranslations)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (department == null)
                return NotFound(new { message = "NotFound" });

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted" });
        }
    }
}