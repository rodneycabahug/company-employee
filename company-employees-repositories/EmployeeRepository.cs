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

    public Employee? GetEmployeeById(Guid companyId, Guid employeeId, bool trackChanges) =>
        FindByCondition(e => e.CompanyId == companyId && e.Id == employeeId, trackChanges)
            .SingleOrDefault();

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
        FindByCondition(e => e.CompanyId == companyId, trackChanges)
            .OrderBy(e => e.Name)
            .ToList();
}