using company_employees_service_contracts;
using company_employees_shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using company_employees_presentation.ModelBinders;

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
    public IActionResult GetCompanies()
    {
        var companies = _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "GetCompany")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _serviceManager.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdCompany = _serviceManager.CompanyService.CreateCompany(company);

        return CreatedAtRoute("GetCompany", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({companyIds})", Name = "GetCompaniesByIds")]
    public IActionResult GetCompaniesByIds(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> companyIds)
    {
        var companiesDto = _serviceManager.CompanyService.GetCompaniesByIds(companyIds, trackChanges: false);
        return Ok(companiesDto);
    }

    [HttpPost("collection")]
    public IActionResult CreateCompanies([FromBody] IEnumerable<CompanyForCreationDto> companies)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var result = _serviceManager.CompanyService.CreateCompanies(companies);
        return CreatedAtRoute("GetCompaniesByIds", new { companyIds = result.companyIds }, result.companies);
    }

    [HttpDelete("{companyId:guid}")]
    public IActionResult DeleteCompany(Guid companyId)
    {
        _serviceManager.CompanyService.DeleteCompany(companyId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{companyId:guid}")]
    public IActionResult UpdateCompany(Guid companyId, CompanyForUpdateDto company)
    {
        if (company is null)
            return BadRequest("CompanyForUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _serviceManager.CompanyService.UpdateCompany(companyId, company, trackChanges: true);
        
        return NoContent();
    }
}