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
        private readonly INotificationService _notificationService;

        public InformationsController(ApexDbContext context, IMapper mapper, INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
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

            try
            {
                await _notificationService.SendNotificationAsync(
                    title: "Yeni Məlumat",
                    body: "Yeni məlumat əlavə edildi.",
                    type: "information",
                    referenceId: entity.Id
                );
            }
            catch (Exception ex) { _ = ex; }

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

            try
            {
                await _notificationService.SendNotificationAsync(
                    title: "Məlumat Yeniləndi",
                    body: $"#{dto.Id} nömrəli məlumat yeniləndi.",
                    type: "information",
                    referenceId: dto.Id
                );
            }
            catch (Exception ex) { _ = ex; }

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

            try
            {
                await _notificationService.SendNotificationAsync(
                    title: "Məlumat Silindi",
                    body: $"#{id} nömrəli məlumat silindi.",
                    type: "information",
                    referenceId: id
                );
            }
            catch (Exception ex) { _ = ex; }

            return Ok(new { message = "Məlumat uğurla silindi" });
        }

    }
}