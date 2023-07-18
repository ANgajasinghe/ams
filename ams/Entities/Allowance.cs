using ams.Entities.Common;
using ams.Enums;
using CsvHelper.Configuration.Attributes;

namespace ams.Entities;

public class Allowance : BaseEntity
{
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public DateTime Date { get; set; }
    public int Amount { get; set; }
    public AllowanceStatus Status { get; set; }
}