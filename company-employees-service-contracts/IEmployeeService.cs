using company_employees_entities.Models;
using company_employees_shared.DataTransferObjects;

namespace company_employees_service_contracts;
public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto? GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    void DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges);
    void UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool comTrackChanges, bool empTrackChanges);
    (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(
        Guid companyId, Guid employeeId, bool comTrackChanges, bool empTrackChanges);
    void SaveChangesForPatch(EmployeeForUpdateDto employeToPatch, Employee employee);
}
