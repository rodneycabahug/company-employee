using System.Linq.Expressions;
using company_employees_entities.Models;

namespace company_employees_contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<Employee?> GetEmployeeByIdAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployeeForCompany(Employee employee);
}