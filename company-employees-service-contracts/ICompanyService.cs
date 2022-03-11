using company_employees_shared.DataTransferObjects;

namespace company_employees_service_contracts;
public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto? GetCompany(Guid companyId, bool trackChanges);
    CompanyDto CreateCompany(CompanyForCreationDto company);
    IEnumerable<CompanyDto> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges);
    (IEnumerable<CompanyDto> companies, string companyIds) CreateCompanies(IEnumerable<CompanyForCreationDto> companies);
    void DeleteCompany(Guid companyId, bool trackChanges);
    void UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges);
}
