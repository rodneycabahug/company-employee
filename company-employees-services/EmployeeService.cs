using AutoMapper;
using company_employees_contracts;
using company_employees_entities.Exceptions;
using company_employees_entities.Models;
using company_employees_service_contracts;
using company_employees_shared.DataTransferObjects;

namespace company_employees_services;
public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public EmployeeService(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        
        var employeeEntity = _mapper.Map<Employee>(employee);
        _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        _repositoryManager.Save();

        var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeDto;
    }

    public void DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = _repositoryManager.Employee.GetEmployeeById(companyId,  employeeId, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        _repositoryManager.Employee.DeleteEmployeeForCompany(employee);
        _repositoryManager.Save();
    } 

    public EmployeeDto? GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = _repositoryManager.Employee.GetEmployeeById(companyId, employeeId, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(companyId);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid employeeId, bool comTrackChanges, bool empTrackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, comTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = _repositoryManager.Employee.GetEmployeeById(companyId, employeeId, empTrackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);
        
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);

        return (employeeToPatch, employee);
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employees = _repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employeesDto;
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeToPatch, Employee employee)
    {
        _mapper.Map(employeToPatch, employee);
        _repositoryManager.Save();
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool comTrackChanges, bool empTrackChanges)
    {
        var companyEntity = _repositoryManager.Company.GetCompany(companyId, comTrackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = _repositoryManager.Employee.GetEmployeeById(companyId, employeeId, empTrackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(employeeId);

        _mapper.Map(employee, employeeEntity);
        _repositoryManager.Save();
    }
}
