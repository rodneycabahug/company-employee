using company_employees_presentation.ActionFilters;
using company_employees_service_contracts;
using company_employees_shared.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace company_employees_presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public EmployeesController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompanyAsync(Guid companyId)
    {
        var companies = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{employeeId:guid}", Name = nameof(GetEmployeeForCompanyAsync))]
    public async Task<IActionResult> GetEmployeeForCompanyAsync(Guid companyId, Guid employeeId)
    {
        var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompanyAsync([FromRoute] Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        var createdEmployee = await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);

        return CreatedAtRoute(nameof(GetEmployeeForCompanyAsync), new { companyId, employeeId = createdEmployee.Id }, createdEmployee);
    }

    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompanyAsync(Guid companyId, Guid employeeId)
    {
        await _serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, employeeId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{employeeId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompanyAsync(Guid companyId, Guid employeeId, [FromBody] EmployeeForUpdateDto employee)
    {
        await _serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, employeeId, employee, comTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }

    [HttpPatch("{employeeId:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompanyAsync(Guid companyId, Guid employeeId, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("JsonPatchDocument object is null");

        var result = await _serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, employeeId, comTrackChanges: false, empTrackChanges: true);

        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _serviceManager.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
}
