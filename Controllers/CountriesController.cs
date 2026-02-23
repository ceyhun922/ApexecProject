using ApexWebAPI.Common;
using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/{lang}/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IStringLocalizer<CountriesController> _localizer;

        public CountriesController(ICountryService countryService, IStringLocalizer<CountriesController> localizer)
        {
            _countryService = countryService;
            _localizer = localizer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultCountryDto>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultCountryDto>>>> GetAll(string lang)
        {
            var result = await _countryService.GetAllAsync(lang);
            return Ok(ApiResponse<IEnumerable<ResultCountryDto>>.Ok(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetByIdCountryDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse<GetByIdCountryDto>>> GetById(string lang, int id)
        {
            var dto = await _countryService.GetByIdAsync(lang, id);
            if (dto == null)
                return NotFound(ApiResponse.Fail(404, _localizer["NotFound"].Value));

            return Ok(ApiResponse<GetByIdCountryDto>.Ok(dto));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Create(CreateCountryDto dto)
        {
            await _countryService.CreateAsync(dto);
            return StatusCode(201, ApiResponse.Ok(_localizer["Created"].Value));
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update(UpdateCountryDto dto)
        {
            await _countryService.UpdateAsync(dto);
            return Ok(ApiResponse.Ok(_localizer["Updated"].Value));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(int id)
        {
            await _countryService.DeleteAsync(id);
            return Ok(ApiResponse.Ok(_localizer["Deleted"].Value));
        }
    }
}
