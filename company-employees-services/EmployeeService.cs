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

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        
        var employeeEntity = _mapper.Map<Employee>(employee);
        _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repositoryManager.SaveAsync();

        var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeDto;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = await _repositoryManager.Employee.GetEmployeeByIdAsync(companyId,  employeeId, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        _repositoryManager.Employee.DeleteEmployeeForCompany(employee);
        await _repositoryManager.SaveAsync();
    } 

    public async Task<EmployeeDto?> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = await _repositoryManager.Employee.GetEmployeeByIdAsync(companyId, employeeId, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(companyId);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid employeeId, bool comTrackChanges, bool empTrackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, comTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = await _repositoryManager.Employee.GetEmployeeByIdAsync(companyId, employeeId, empTrackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);
        
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);

        return (employeeToPatch, employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employees = await _repositoryManager.Employee.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employeesDto;
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeToPatch, Employee employee)
    {
        _mapper.Map(employeToPatch, employee);
        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(
        Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool comTrackChanges, bool empTrackChanges)
    {
        var companyEntity = await _repositoryManager.Company.GetCompanyAsync(companyId, comTrackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = await _repositoryManager.Employee.GetEmployeeByIdAsync(companyId, employeeId, empTrackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(employeeId);

        _mapper.Map(employee, employeeEntity);
        await _repositoryManager.SaveAsync();
    }
}
