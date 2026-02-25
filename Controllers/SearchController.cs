using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [ProducesResponseType(typeof(SearchResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SearchResultDto>> Search([FromRoute] string lang, [FromQuery] SearchFilterDto filter)
        {
            var validation = await _validator.ValidateAsync(filter);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var query = _context.Universities!
                .Include(u => u.Translations)
                .Include(u => u.Country)
                    .ThenInclude(c => c!.CountryTranslations)
                .Include(u => u.EducationLevel)
                    .ThenInclude(e => e!.EducationLevelTranslations)
                .Include(u => u.Department)
                    .ThenInclude(d => d!.DepartmentTranslations)
                .Where(u => u.Status)
                .AsQueryable();

            if (filter.CountryId.HasValue)
                query = query.Where(u => u.CountryId == filter.CountryId.Value);

            if (filter.EducationLevelId.HasValue)
                query = query.Where(u => u.EducationLevelId == filter.EducationLevelId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(u => u.DepartmentId == filter.DepartmentId.Value);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return NotFound(new { message = "Nəticə tapılmadı" });

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var results = items.Select(u =>
            {
                var t = u.Translations.FirstOrDefault(x => x.Language == lang)
                    ?? u.Translations.FirstOrDefault(x => x.Language == "az");

                return new SearchItemDto
                {
                    UniversityId = u.Id,
                    Title = t?.Title,
                    SubTitle = t?.SubTitle,
                    Description = t?.Description,
                    ImageUrl = u.ImageUrl,
                    Country = new ResultCountryDto
                    {
                        Id = u.Country!.Id,
                        Name = u.Country.CountryTranslations?
                            .FirstOrDefault(x => x.Language == lang)?.Name
                            ?? u.Country.CountryTranslations?.FirstOrDefault(x => x.Language == "az")?.Name
                    },
                    EducationLevel = new ResultEducationLevelDto
                    {
                        Id = u.EducationLevel!.Id,
                        CountryId = u.EducationLevel.CountryId,
                        Name = u.EducationLevel.EducationLevelTranslations?
                            .FirstOrDefault(x => x.Language == lang)?.Name
                            ?? u.EducationLevel.EducationLevelTranslations?.FirstOrDefault(x => x.Language == "az")?.Name
                    },
                    Department = new ResultDepartmentDto
                    {
                        Id = u.Department!.Id,
                        EducationLevelId = u.Department.EducationLevelId,
                        Name = u.Department.DepartmentTranslations?
                            .FirstOrDefault(x => x.Language == lang)?.Name
                            ?? u.Department.DepartmentTranslations?.FirstOrDefault(x => x.Language == "az")?.Name
                    }
                };
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