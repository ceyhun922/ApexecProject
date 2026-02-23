using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.ContactInfoDTOs;
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

        public async Task<IEnumerable<ResultContactInfoDto>> GetAllAsync()
        {
            var contacts = await _context.ContactInfos!.ToListAsync();
            return _mapper.Map<List<ResultContactInfoDto>>(contacts);
        }

        public async Task<GetByIdContactDto?> GetByIdAsync(int id)
        {
            var contact = await _context.ContactInfos!.FindAsync(id);
            return contact == null ? null : _mapper.Map<GetByIdContactDto>(contact);
        }

        public async Task CreateAsync(CreateContactInfoDto dto)
        {
            var contact = _mapper.Map<ContactInfo>(dto);
            await _context.ContactInfos!.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateContactInfoDto dto)
        {
            var contact = await _context.ContactInfos!.FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("ContactInfo not found");

            _mapper.Map(dto, contact);
            _context.ContactInfos.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await _context.ContactInfos!.FindAsync(id)
                ?? throw new KeyNotFoundException($"ContactInfo {id} not found");

            _context.ContactInfos.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}