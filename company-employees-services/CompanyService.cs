using AutoMapper;
using company_employees_contracts;
using company_employees_entities.Exceptions;
using company_employees_entities.Models;
using company_employees_service_contracts;
using company_employees_shared.DataTransferObjects;

namespace company_employees_services;
public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public CompanyService(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public (IEnumerable<CompanyDto> companies, string companyIds) CreateCompanies(IEnumerable<CompanyForCreationDto> companies)
    {
        if (companies is null || !companies.Any())
            throw new CompanyCollectionBadRequestException();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);
        foreach (var companyEntity in companyEntities)
        {
            _repositoryManager.Company.CreateCompany(companyEntity);
        }

        _repositoryManager.Save();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var companyIds = string.Join(",", companyDtos.Select(c => c.Id));

        return (companies: companyDtos, companyIds: companyIds);
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repositoryManager.Company.CreateCompany(companyEntity);
        _repositoryManager.Save();

        var companyDto = _mapper.Map<CompanyDto>(companyEntity);

        return companyDto;
    }

    public void DeleteCompany(Guid companyId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        
        _repositoryManager.Company.DeleteCompany(company);
        _repositoryManager.Save();
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = _repositoryManager.Company.GetAllCompanies(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public IEnumerable<CompanyDto> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds is null || !companyIds.Any())
            throw new IdParametersBadRequestException();

        var companies = _repositoryManager.Company.GetCompaniesByIds(companyIds, trackChanges);
        
        if (companies.Count() != companyIds.Count())
            throw new CollectionCountMismatchBadRequestException();
        
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public CompanyDto? GetCompany(Guid id, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(id, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(id);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public void UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges)
    {
        var companyEntity = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        _mapper.Map(company, companyEntity);
        
        _repositoryManager.Save();
    }
}
