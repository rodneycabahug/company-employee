using System.Dynamic;
using AutoMapper;
using company_employees_contracts;
using company_employees_entities.Exceptions;
using company_employees_entities.Models;
using company_employees_service_contracts;
using company_employees_shared.DataTransferObjects;
using company_employees_shared.RequestFeatures;

namespace company_employees_services;
public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeDto> _dataShaper;

    public EmployeeService(
        ILoggerManager loggerManager,
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IDataShaper<EmployeeDto> dataShaper)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employee);
        _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repositoryManager.SaveAsync();

        var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeDto;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, trackChanges);

        _repositoryManager.Employee.DeleteEmployeeForCompany(employee);
        await _repositoryManager.SaveAsync();
    }

    public async Task<EmployeeDto?> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, trackChanges);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid employeeId, bool comTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, comTrackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, empTrackChanges);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);

        return (employeeToPatch, employee);
    }

    public async Task<(IEnumerable<ExpandoObject> employees, Metadata metadata)> GetEmployeesAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
        {
            throw new MaxAgeRangeBadRequestException();
        }

        await CheckIfCompanyExistsAsync(companyId, trackChanges);

        var employeesWithMetadata = await _repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetadata);
        var shapedEmployeesDto = _dataShaper.ShapeData(employeesDto, employeeParameters.Fields);

        return (employees: shapedEmployeesDto, metadata: employeesWithMetadata.Metadata);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeToPatch, Employee employee)
    {
        _mapper.Map(employeToPatch, employee);
        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(
        Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool comTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, comTrackChanges);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, empTrackChanges);

        _mapper.Map(employee, employeeEntity);
        await _repositoryManager.SaveAsync();
    }

    private async Task CheckIfCompanyExistsAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var employee = await _repositoryManager.Employee.GetEmployeeByIdAsync(companyId, employeeId, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        return employee;
    }
}
