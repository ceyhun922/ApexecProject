using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.DTOs.SearchDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SearchController : ControllerBase
    {
        private readonly ApexDbContext _context;

        public SearchController(ApexDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<SearchResultDto>> Search([FromQuery] SearchFilterDto filter)
        {
            var query = _context.Departments
                .Include(d => d.DepartmentTranslations)
                .Include(d => d.EducationLevel)
                    .ThenInclude(e => e!.EducationLevelTranslations)
                .Include(d => d.EducationLevel)
                    .ThenInclude(e => e!.Country)
                        .ThenInclude(c => c!.CountryTranslations)
                .AsQueryable();

            if (filter.CountryId.HasValue)
                query = query.Where(d => d.EducationLevel!.CountryId == filter.CountryId.Value);

            if (filter.EducationLevelId.HasValue)
                query = query.Where(d => d.EducationLevelId == filter.EducationLevelId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(d => d.Id == filter.DepartmentId.Value);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return NotFound(new { message = "Sonuç bulunamadı" });

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var results = items.Select(d => new SearchItemDto
            {
                Country = new ResultCountryDto
                {
                    Id = d.EducationLevel!.Country!.Id,
                    Name = d.EducationLevel.Country.CountryTranslations?
                        .FirstOrDefault(t => t.Language == filter.Language)?.Name
                },
                EducationLevel = new ResultEducationLevelDto
                {
                    Id = d.EducationLevel.Id,
                    CountryId = d.EducationLevel.CountryId,
                    Name = d.EducationLevel.EducationLevelTranslations?
                        .FirstOrDefault(t => t.Language == filter.Language)?.Name
                },
                Department = new ResultDepartmentDto
                {
                    Id = d.Id,
                    EducationLevelId = d.EducationLevelId,
                    Name = d.DepartmentTranslations?
                        .FirstOrDefault(t => t.Language == filter.Language)?.Name
                }
            }).ToList();

            return Ok(new SearchResultDto
            {
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                Results = results
            });
        }
    }
}