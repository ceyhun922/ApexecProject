using ApexWebAPI.Concrete;
using ApexWebAPI.DTOs.FooterDTOs;
using ApexWebAPI.Entities;
using ApexWebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexWebAPI.Services.Concrete
{
    public class FooterService : IFooterService
    {
        private readonly ApexDbContext _context;
        private readonly IMapper _mapper;

        public FooterService(ApexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultFooterDto>> GetAllAsync()
        {
            var footers = await _context.Footers!.ToListAsync();
            return _mapper.Map<List<ResultFooterDto>>(footers);
        }

        public async Task<GetByIdFooterDto?> GetByIdAsync(int id)
        {
            var footer = await _context.Footers!.FindAsync(id);
            return footer == null ? null : _mapper.Map<GetByIdFooterDto>(footer);
        }

        public async Task CreateAsync(CreateFooterDto dto)
        {
            var footer = _mapper.Map<Footer>(dto);
            await _context.Footers!.AddAsync(footer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateFooterDto dto)
        {
            var footer = await _context.Footers!.FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("Footer not found");

            _mapper.Map(dto, footer);
            _context.Footers.Update(footer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var footer = await _context.Footers!.FindAsync(id)
                ?? throw new KeyNotFoundException($"Footer {id} not found");

            _context.Footers.Remove(footer);
            await _context.SaveChangesAsync();
        }
    }
}