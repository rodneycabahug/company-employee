using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using company_employees_contracts;
using company_employees_entities.Models;

namespace company_employees_repositories;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(c => c.Name).ToList();

    public IEnumerable<Company> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges) =>
        FindByCondition(x => companyIds.Contains(x.Id), trackChanges)
            .ToList();

    public Company? GetCompany(Guid companyId, bool trackChanges) =>
        FindByCondition(c => c.Id == companyId, trackChanges)
            .SingleOrDefault();
}