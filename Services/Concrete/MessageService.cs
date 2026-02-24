using ApexWebAPI.Common;
using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.MessageDTOs.cs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApexWebAPI.Common.HtmlSanitizerHelper;

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
            var items = await _context.Messages!.ToListAsync();
            return _mapper.Map<List<ResultMessageDto>>(items);
        }

        public async Task<GetByIdMessageDto?> GetByIdAsync(int id)
        {
            var item = await _context.Messages!.FindAsync(id);
            return item == null ? null : _mapper.Map<GetByIdMessageDto>(item);
        }

        public async Task CreateAsync(CreateMessageDto dto)
        {
            var item = _mapper.Map<Message>(dto);
            item.FullName = Sanitize(item.FullName);
            item.Messagee = Sanitize(item.Messagee);
            _context.Messages!.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateMessageDto dto)
        {
            var item = await _context.Messages!.FindAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Message {dto.Id} not found");

            _mapper.Map(dto, item);
            item.FullName = Sanitize(item.FullName);
            item.Messagee = Sanitize(item.Messagee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Messages!.FindAsync(id)
                ?? throw new KeyNotFoundException($"Message {id} not found");

            _context.Messages.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}