using company_employees_service_contracts;
using company_employees_shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using company_employees_presentation.ModelBinders;
using company_employees_presentation.ActionFilters;

namespace company_employees_presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CompaniesController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompaniesAsync()
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = nameof(GetCompanyAsync))]
    public async Task<IActionResult> GetCompanyAsync(Guid id)
    {
        var company = await _serviceManager.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CompanyForCreationDto company)
    {
        var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);

        return CreatedAtRoute(nameof(GetCompanyAsync), new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({companyIds})", Name = nameof(GetCompaniesByIdsAsync))]
    public async Task<IActionResult> GetCompaniesByIdsAsync(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> companyIds)
    {
        var companiesDto = await _serviceManager.CompanyService.GetCompaniesByIdsAsync(companyIds, trackChanges: false);
        return Ok(companiesDto);
    }

    [HttpPost("collection")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompaniesAsync([FromBody] IEnumerable<CompanyForCreationDto> companies)
    {
        var result = await _serviceManager.CompanyService.CreateCompaniesAsync(companies);
        return CreatedAtRoute(nameof(GetCompaniesByIdsAsync), new { companyIds = result.companyIds }, result.companies);
    }

    [HttpDelete("{companyId:guid}")]
    public async Task<IActionResult> DeleteCompanyAsync(Guid companyId)
    {
        await _serviceManager.CompanyService.DeleteCompanyAsync(companyId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{companyId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto company)
    {
        await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, company, trackChanges: true);

        return NoContent();
    }
}
