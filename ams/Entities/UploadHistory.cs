using ams.Entities.Common;
using ams.Enums;

namespace ams.Entities;

public class UploadHistory : BaseEntity
{

    public UploadHistory()
    {
            
    }
    
    public static UploadHistory Create(string fileName)
    {
        return new UploadHistory(fileName);
    }
    
    public UploadHistory(string fileName)
    {
        StartedTime = DateTime.Now;
        FileName = fileName;
        Status = UploadStatus.Started;
    }
    
    public void Complete(int effectedRowCount)
    {
        CompletedTime = DateTime.Now;
        Status = UploadStatus.Completed;
        EffectedRowCount = effectedRowCount;
    }
    
    public void Fail()
    {
        CompletedTime = DateTime.Now;
        Status = UploadStatus.Failed;
    }
    
    
    public string FileName { get; set; }
    public DateTime StartedTime { get; set; }
    public UploadStatus Status { get; set; }
    public DateTime CompletedTime { get; set; }
    public int EffectedRowCount { get; set; }
}