using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.InformationDTOs;
using ApexWebAPI.DTOs.MessageDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
            var items = await _context.Contacts!.ToListAsync();
            return _mapper.Map<List<ResultInformationDto>>(items);
        }

        public async Task<GetByIdInformationDto?> GetByIdAsync(int id)
        {
            var item = await _context.Contacts!.FindAsync(id);
            return item == null ? null : _mapper.Map<GetByIdInformationDto>(item);
        }

        public async Task CreateAsync(CreateInformationDto dto)
        {
            var contact = _mapper.Map<Contact>(dto);
            _context.Contacts!.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateInformationDto dto)
        {
            var item = await _context.Contacts!.FindAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Information {dto.Id} not found");

            _mapper.Map(dto, item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Contacts!.FindAsync(id)
                ?? throw new KeyNotFoundException($"Information {id} not found");

            _context.Contacts.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
