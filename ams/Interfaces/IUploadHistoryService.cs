using ams.Entities;

namespace ams.Interfaces;

public interface IUploadHistoryService
{
    public UploadHistory Build(string fileName);
    Task AddCompletedUploadHistoryAsync(UploadHistory uploadHistory, int effectedRowCount);
    Task AddFailedUploadHistoryAsync(UploadHistory uploadHistory);
}