﻿using AutoMapper;
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

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);

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
