using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using company_employees_contracts;
using company_employees_entities.Models;
using company_employees_shared.RequestFeatures;

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

    public async Task<PagedList<Employee>> GetEmployeesAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        var employees = await FindByCondition(e =>
            e.CompanyId == companyId
            && (e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge),
            trackChanges)
            .OrderBy(e => e.Name)
            .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            .Take(employeeParameters.PageSize)
            .ToListAsync();

        var count = await FindByCondition(e => e.CompanyId == companyId, trackChanges).CountAsync();

        return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
    }
}
