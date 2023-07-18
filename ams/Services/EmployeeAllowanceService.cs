using ams.Entities;
using ams.Interfaces;
using ams.Models;
using Microsoft.EntityFrameworkCore;

namespace ams.Services;

public class EmployeeAllowanceService : IEmployeeAllowanceService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUploadHistoryService _uploadHistoryService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly AppDbContext _dbContext;

    [ActivatorUtilitiesConstructor]
    public EmployeeAllowanceService(IServiceProvider serviceProvider, 
        IUploadHistoryService uploadHistoryService, 
        IWebHostEnvironment webHostEnvironment,AppDbContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _uploadHistoryService = uploadHistoryService;
        _webHostEnvironment = webHostEnvironment;
        _dbContext = dbContext;
    }
    
    public async Task<List<Allowance>> GetAllowancesAsync() 
        => await _dbContext.Allowances.ToListAsync();
    

    public async Task<UploadHistory> UploadAsync(IFormFile file)
    {
        var uploadHistory = _uploadHistoryService.Build(file.FileName);
        var filePath = await SaveFileToLocationAsync(file);

        Task.Run(async () =>
        {
            try
            {
                Console.WriteLine("Start Uploading...");
                // get required services
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var uploadHistoryService = scope.ServiceProvider.GetRequiredService<IUploadHistoryService>();
                var csvService = scope.ServiceProvider.GetRequiredService<ICsvService>();
                
                var allowances = AllowanceModel.GetAllowances(csvService.ReadCsvFileAsync<AllowanceModel>(filePath));
                await Task.Delay(5000); // simulate long running task
                
                // save to db
                await dbContext.Allowances.AddRangeAsync(allowances);
                await uploadHistoryService.AddCompletedUploadHistoryAsync(uploadHistory, allowances.Count);
                await dbContext.SaveChangesAsync();
                
                await Task.Delay(5000); // simulate long database insertion
                
                DeleteFileFromLocation(filePath);
                await DisposeResourcesAsync(scope, dbContext);
                Console.WriteLine("Upload Completed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to upload");
                Console.WriteLine(e);
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var uploadHistoryService = scope.ServiceProvider.GetRequiredService<IUploadHistoryService>();
                
                await uploadHistoryService.AddFailedUploadHistoryAsync(uploadHistory);
                await dbContext.SaveChangesAsync();

                DeleteFileFromLocation(filePath);
                await DisposeResourcesAsync(scope, dbContext);
                throw;
            }
        });

        return await Task.FromResult(uploadHistory);
    }
    
    // dispose resources
    private async Task DisposeResourcesAsync(IServiceScope scope, AppDbContext dbContext)
    {
        await dbContext.DisposeAsync();
        scope.Dispose();
    }
    
    // save file to location and return file path
    private async Task<string> SaveFileToLocationAsync(IFormFile file)
    {
        try
        {
            var dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Upload");
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            var filePath = Path.Combine(dirPath, Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName));
            
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();
            
            return filePath;
        }
        catch
        {
            return string.Empty;
        }
    }
    
    // delete file from location
    private bool DeleteFileFromLocation(string filePath)
    {
        try
        {
            if (!File.Exists(filePath)) 
                return false;
            File.Delete(filePath);
            return true;
        }
        catch
        {
            return false;
        }
    }
}