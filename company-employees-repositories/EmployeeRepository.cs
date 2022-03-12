using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using company_employees_contracts;
using company_employees_entities.Models;

namespace company_employees_repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployeeForCompany(Employee employee) =>
        Delete(employee);

    public async Task<Employee?> GetEmployeeByIdAsync(Guid companyId, Guid employeeId, bool trackChanges) =>
        await FindByCondition(e => e.CompanyId == companyId && e.Id == employeeId, trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges) =>
        await FindByCondition(e => e.CompanyId == companyId, trackChanges)
            .OrderBy(e => e.Name)
            .ToListAsync();
}