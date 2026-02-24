using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.InformationDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InformationsController : ControllerBase
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;
        public InformationsController(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet("information-list")]
        [ProducesResponseType(typeof(List<ResultInformationDto>), 200)]
        public async Task<ActionResult<List<ResultInformationDto>>> GetMessages()
        {
            var messages = await _context.Informations.ToListAsync();

            var dto = _mapper.Map<List<ResultInformationDto>>(messages);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdInformationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetByIdInformationDto>> GetByIdMessage(int id)
        {
            var message = await _context.Informations.FindAsync(id);

            if (message == null)
                return NotFound(new { message = "Məlumat tapılmadı" });

            var mapper = _mapper.Map<GetByIdInformationDto>(message);
            return Ok(mapper);
        }

        [HttpPost("information-create")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> SubmitMessage([FromBody] CreateInformationDto informationDto)
        {
            var entity = _mapper.Map<Information>(informationDto);
            _context.Informations.Add(entity);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Məlumat uğurla göndərildi" });
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateInformationDto dto)
        {
            var message = await _context.Informations.FindAsync(dto.Id);

            if (message == null)
                return NotFound(new { message = "Məlumat tapılmadı" });

            _mapper.Map(dto, message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Məlumat uğurla yeniləndi" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Informations.FindAsync(id);

            if (message == null)
                return NotFound(new { message = "Məlumat tapılmadı" });

            _context.Informations.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Məlumat uğurla silindi" });
        }

    }
}