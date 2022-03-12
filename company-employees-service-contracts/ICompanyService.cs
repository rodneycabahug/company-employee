using company_employees_shared.DataTransferObjects;

namespace company_employees_service_contracts;
public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
    Task<CompanyDto?> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
    Task<IEnumerable<CompanyDto>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);
    Task<(IEnumerable<CompanyDto> companies, string companyIds)> CreateCompaniesAsync(IEnumerable<CompanyForCreationDto> companies);
    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
    Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto company, bool trackChanges);
}
