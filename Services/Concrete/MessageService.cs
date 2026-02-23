using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.MessageDTOs.cs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class MessageService : IMessageService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public MessageService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultMessageDto>> GetAllAsync()
        {
            var items = await _context.Contacts!.ToListAsync();
            return _mapper.Map<List<ResultMessageDto>>(items);
        }

        public async Task<GetByIdMessageDto?> GetByIdAsync(int id)
        {
            var item = await _context.Contacts!.FindAsync(id);
            return item == null ? null : _mapper.Map<GetByIdMessageDto>(item);
        }

        public async Task CreateAsync(CreateMessageDto dto)
        {
            var contact = _mapper.Map<Contact>(dto);
            _context.Contacts!.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateMessageDto dto)
        {
            var item = await _context.Contacts!.FindAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Message {dto.Id} not found");

            _mapper.Map(dto, item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Contacts!.FindAsync(id)
                ?? throw new KeyNotFoundException($"Message {id} not found");

            _context.Contacts.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
