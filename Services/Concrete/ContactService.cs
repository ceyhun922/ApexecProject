using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.ContactDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class ContactService : IContactService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public ContactService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultContactDto>> GetAllAsync()
        {
            var contacts = await _context.Contacts!.ToListAsync();
            return _mapper.Map<List<ResultContactDto>>(contacts);
        }

        public async Task<GetByIdContactDto?> GetByIdAsync(int id)
        {
            var contact = await _context.Contacts!.FindAsync(id);
            return contact == null ? null : _mapper.Map<GetByIdContactDto>(contact);
        }

        public async Task CreateAsync(CreateContactDto dto)
        {
            var contact = _mapper.Map<Contact>(dto);
            contact.ImageUrl = dto.ImageUrl;
            await _context.Contacts!.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateContactDto dto)
        {
            var contact = await _context.Contacts!.FindAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Contact {dto.Id} not found");

            _mapper.Map(dto, contact);
            contact.ImageUrl = dto.ImageUrl;
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await _context.Contacts!.FindAsync(id)
                ?? throw new KeyNotFoundException($"Contact {id} not found");

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}
