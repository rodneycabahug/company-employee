using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using company_employees_contracts;
using company_employees_entities.Models;

namespace company_employees_repositories;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}

    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
        await FindAll(trackChanges).OrderBy(c => c.Name)
            .ToListAsync();

    public async Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges) =>
        await FindByCondition(x => companyIds.Contains(x.Id), trackChanges)
            .ToListAsync();

    public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges) =>
        await FindByCondition(c => c.Id == companyId, trackChanges)
            .SingleOrDefaultAsync();
}