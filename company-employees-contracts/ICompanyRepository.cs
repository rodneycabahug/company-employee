using System.Linq.Expressions;
using company_employees_entities.Models;

namespace company_employees_contracts;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Company? GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    IEnumerable<Company> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges);
    void DeleteCompany(Company company);
}