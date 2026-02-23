using ApexWebAPI.DTOs.SummerSchoolDTOs;

namespace ApexWebAPI.Services.Interfaces
{
    public interface ISummerSchoolService
    {
        Task<IEnumerable<ResultSummerSchoolDto>> GetAllAsync(string lang);
        Task CreateAsync(CreateSummerSchoolDto dto);
        Task UpdateAsync(UpdateSummerSchoolDto dto);
    }
}
