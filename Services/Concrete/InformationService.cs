using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.InformationDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApexWebAPI.Common.HtmlSanitizerHelper;

namespace ApexWebAPI.Services.Concrete
{
    public class InformationService : IInformationService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public InformationService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultInformationDto>> GetAllAsync()
        {
            var items = await _context.Informations!.ToListAsync();
            return _mapper.Map<List<ResultInformationDto>>(items);
        }

        public async Task<GetByIdInformationDto?> GetByIdAsync(int id)
        {
            var item = await _context.Informations!.FindAsync(id);
            return item == null ? null : _mapper.Map<GetByIdInformationDto>(item);
        }

        public async Task CreateAsync(CreateInformationDto dto)
        {
            var item = _mapper.Map<Information>(dto);
            item.FullName = Sanitize(item.FullName);
            item.Education = Sanitize(item.Education);
            item.ClassOrYear = Sanitize(item.ClassOrYear);
            _context.Informations!.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateInformationDto dto)
        {
            var item = await _context.Informations!.FindAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Information {dto.Id} not found");

            _mapper.Map(dto, item);
            item.FullName = Sanitize(item.FullName);
            item.Education = Sanitize(item.Education);
            item.ClassOrYear = Sanitize(item.ClassOrYear);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Informations!.FindAsync(id)
                ?? throw new KeyNotFoundException($"Information {id} not found");

            _context.Informations.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}