using ams.Entities;
using ams.Interfaces;

namespace ams.Services;

public class UploadHistoryService : IUploadHistoryService
{
    private readonly AppDbContext _dbContext;

    public UploadHistoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public  UploadHistory Build(string fileName)
    {
        var uploadHistory = UploadHistory.Create(fileName);
        return uploadHistory;
    }
    
    public async Task AddCompletedUploadHistoryAsync(UploadHistory uploadHistory, int effectedRowCount)
    {
        uploadHistory.Complete(effectedRowCount);
        await _dbContext.UploadHistories.AddAsync(uploadHistory);
    }
    public async Task AddFailedUploadHistoryAsync(UploadHistory uploadHistory)
    {
        uploadHistory.Fail();
        await _dbContext.UploadHistories.AddAsync(uploadHistory);
    }
    
}