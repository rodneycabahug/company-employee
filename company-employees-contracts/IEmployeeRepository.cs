using System.Linq.Expressions;
using company_employees_entities.Models;
using company_employees_shared.RequestFeatures;

namespace company_employees_contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployeesAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<Employee?> GetEmployeeByIdAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployeeForCompany(Employee employee);
}
