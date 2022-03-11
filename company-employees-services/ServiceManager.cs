using AutoMapper;
using company_employees_contracts;
using company_employees_service_contracts;

namespace company_employees_services;
public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(
        ILoggerManager loggerManager,
        IRepositoryManager repositoryManager,
        IMapper mapper)
    {
        _companyService = new Lazy<ICompanyService>(() =>
            new CompanyService(loggerManager, repositoryManager, mapper));
        _employeeService = new Lazy<IEmployeeService>(() =>
            new EmployeeService(loggerManager, repositoryManager, mapper));
    }

    public ICompanyService CompanyService => _companyService.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
}
