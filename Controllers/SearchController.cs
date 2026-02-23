using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;
using ApexWebAPI.DTOs.SearchDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IValidator<SearchFilterDto> _validator;

        public SearchController(ApexDbContext context, IValidator<SearchFilterDto> validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SearchResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SearchResultDto>> Search([FromRoute] string lang, [FromQuery] SearchFilterDto filter)
        {
            var validation = await _validator.ValidateAsync(filter);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

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
                        .FirstOrDefault(t => t.Language == lang)?.Name
                },
                EducationLevel = new ResultEducationLevelDto
                {
                    Id = d.EducationLevel.Id,
                    CountryId = d.EducationLevel.CountryId,
                    Name = d.EducationLevel.EducationLevelTranslations?
                        .FirstOrDefault(t => t.Language == lang)?.Name
                },
                Department = new ResultDepartmentDto
                {
                    Id = d.Id,
                    EducationLevelId = d.EducationLevelId,
                    Name = d.DepartmentTranslations?
                        .FirstOrDefault(t => t.Language == lang)?.Name
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