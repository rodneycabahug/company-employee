using company_employees_entities.Models;
using company_employees_shared.DataTransferObjects;

namespace company_employees_service_contracts;
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<EmployeeDto?> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool comTrackChanges, bool empTrackChanges);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid employeeId, bool comTrackChanges, bool empTrackChanges);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeToPatch, Employee employee);
}
