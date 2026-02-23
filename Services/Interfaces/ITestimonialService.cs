using ApexWebAPI.DTOs.TestimonialDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface ITestimonialService
    {
        Task<IEnumerable<ResultTestimonialDto>> GetAllAsync(string lang);
        Task<GetByIdTestimonialDto?> GetByIdAsync(string lang, int id);
        Task CreateAsync(CreateTestimonialDto dto);
        Task UpdateAsync(UpdateTestimonialDto dto);
        Task DeleteAsync(int id);
    }
}
