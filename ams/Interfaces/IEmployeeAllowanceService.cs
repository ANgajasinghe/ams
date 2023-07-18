using ams.Entities;

namespace ams.Interfaces;

public interface IEmployeeAllowanceService
{
    Task<UploadHistory> UploadAsync(IFormFile file);
    Task<List<Allowance>> GetAllowancesAsync();
}