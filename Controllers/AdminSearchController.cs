using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.AdminSearchDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/admin/search")]
    public class AdminSearchController : ControllerBase
    {
        private readonly ApexDbContext _context;

        public AdminSearchController(ApexDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdminSearchResultDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AdminSearchResultDto>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                return BadRequest(new { message = "Axtarış üçün minimum 2 simvol daxil edin" });

            var keyword = q.Trim().ToLower();

            var messages = await _context.Messages!
                .Where(m =>
                    (m.FullName != null && m.FullName.ToLower().Contains(keyword)) ||
                    (m.Email != null && m.Email.ToLower().Contains(keyword)) ||
                    (m.PhoneNumber != null && m.PhoneNumber.ToLower().Contains(keyword)) ||
                    (m.Messagee != null && m.Messagee.ToLower().Contains(keyword)))
                .Select(m => new AdminSearchItemDto
                {
                    Id = m.Id,
                    Title = m.FullName,
                    SubTitle = m.Email,
                    Type = "message",
                    Url = $"/messages/{m.Id}"
                })
                .Take(10)
                .ToListAsync();

            var summerSchools = await _context.SummerSchools!
                .Include(s => s.Translations)
                .Where(s => s.Translations.Any(t =>
                    (t.Title != null && t.Title.ToLower().Contains(keyword)) ||
                    (t.SubTitle != null && t.SubTitle.ToLower().Contains(keyword))))
                .Select(s => new AdminSearchItemDto
                {
                    Id = s.Id,
                    Title = s.Translations.FirstOrDefault(t => t.Language == "az") != null
                        ? s.Translations.First(t => t.Language == "az").Title
                        : s.Translations.Select(t => t.Title).FirstOrDefault(),
                    SubTitle = "Yay Məktəbi",
                    Type = "summerschool",
                    Url = $"/summer-schools/{s.Id}"
                })
                .Take(10)
                .ToListAsync();

            var faqs = await _context.Faqs!
                .Where(f =>
                    (f.Title != null && f.Title.ToLower().Contains(keyword)) ||
                    (f.Content != null && f.Content.ToLower().Contains(keyword)))
                .Select(f => new AdminSearchItemDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    SubTitle = "FAQ",
                    Type = "faq",
                    Url = $"/faqs/{f.Id}"
                })
                .Take(10)
                .ToListAsync();

            var countries = await _context.Countries!
                .Include(c => c.CountryTranslations)
                .Where(c => c.CountryTranslations!.Any(t =>
                    t.Name != null && t.Name.ToLower().Contains(keyword)))
                .Select(c => new AdminSearchItemDto
                {
                    Id = c.Id,
                    Title = c.CountryTranslations!.FirstOrDefault(t => t.Language == "az") != null
                        ? c.CountryTranslations!.First(t => t.Language == "az").Name
                        : c.CountryTranslations!.Select(t => t.Name).FirstOrDefault(),
                    SubTitle = "Ölkə",
                    Type = "country",
                    Url = $"/countries/{c.Id}"
                })
                .Take(10)
                .ToListAsync();

            var departments = await _context.Departments!
                .Include(d => d.DepartmentTranslations)
                .Where(d => d.DepartmentTranslations!.Any(t =>
                    t.Name != null && t.Name.ToLower().Contains(keyword)))
                .Select(d => new AdminSearchItemDto
                {
                    Id = d.Id,
                    Title = d.DepartmentTranslations!.FirstOrDefault(t => t.Language == "az") != null
                        ? d.DepartmentTranslations!.First(t => t.Language == "az").Name
                        : d.DepartmentTranslations!.Select(t => t.Name).FirstOrDefault(),
                    SubTitle = "Departament",
                    Type = "department",
                    Url = $"/departments/{d.Id}"
                })
                .Take(10)
                .ToListAsync();

            var abouts = await _context.Abouts!
                .Where(a =>
                    (a.Title != null && a.Title.ToLower().Contains(keyword)) ||
                    (a.SubTitle != null && a.SubTitle.ToLower().Contains(keyword)))
                .Select(a => new AdminSearchItemDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    SubTitle = a.SubTitle,
                    Type = "about",
                    Url = $"/about/{a.Id}"
                })
                .Take(10)
                .ToListAsync();

            var result = new AdminSearchResultDto
            {
                Messages = messages,
                SummerSchools = summerSchools,
                Faqs = faqs,
                Countries = countries,
                Departments = departments,
                Abouts = abouts,
                TotalCount = messages.Count + summerSchools.Count + faqs.Count +
                             countries.Count + departments.Count + abouts.Count
            };

            return Ok(result);
        }
    }
}
