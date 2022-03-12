using System.Linq.Expressions;
using company_employees_entities.Models;

namespace company_employees_contracts;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
    Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);
    void DeleteCompany(Company company);
}
