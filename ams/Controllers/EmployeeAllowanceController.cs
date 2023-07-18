using ams.Entities;
using ams.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ams.Controllers;

[ApiController]
[Route("api/employee-allowances")]
public class EmployeeAllowanceController : ControllerBase
{
    private readonly IEmployeeAllowanceService _allowanceService;
    public EmployeeAllowanceController(IEmployeeAllowanceService allowanceService)
    {
        _allowanceService = allowanceService;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var ret = await _allowanceService.UploadAsync(file);
        return Ok(ret);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Allowance>>> Get() 
        => Ok(await _allowanceService.GetAllowancesAsync());
    
}