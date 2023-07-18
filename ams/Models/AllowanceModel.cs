using ams.Entities;
using ams.Enums;
using CsvHelper.Configuration.Attributes;

namespace ams.Models;

public class AllowanceModel
{
    [Name("Employee ID ")]
    public int EmployeeId { get; set; }
    [Name("Department ID")]
    public int DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public int Amount { get; set; }
    public AllowanceStatus Status { get; set; }

    public static List<Allowance> GetAllowances(List<AllowanceModel> models)
     => models.Select(model => new Allowance
        {
            EmployeeId = model.EmployeeId,
            DepartmentId = model.DepartmentId,
            Date = model.Date,
            Amount = model.Amount,
            Status = model.Status
        }).ToList();
    
}