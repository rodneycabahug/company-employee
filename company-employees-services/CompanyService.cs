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

    public async Task<(IEnumerable<CompanyDto> companies, string companyIds)> CreateCompaniesAsync(
        IEnumerable<CompanyForCreationDto> companies)
    {
        if (companies is null || !companies.Any())
            throw new CompanyCollectionBadRequestException();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companies);
        foreach (var companyEntity in companyEntities)
        {
            _repositoryManager.Company.CreateCompany(companyEntity);
        }

        await _repositoryManager.SaveAsync();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var companyIds = string.Join(",", companyDtos.Select(c => c.Id));

        return (companies: companyDtos, companyIds: companyIds);
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repositoryManager.Company.CreateCompany(companyEntity);

        await _repositoryManager.SaveAsync();

        var companyDto = _mapper.Map<CompanyDto>(companyEntity);

        return companyDto;
    }

    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        _repositoryManager.Company.DeleteCompany(company);

        await _repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repositoryManager.Company.GetAllCompaniesAsync(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public async Task<IEnumerable<CompanyDto>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if (companyIds is null || !companyIds.Any())
            throw new IdParametersBadRequestException();

        var companies = await _repositoryManager.Company.GetCompaniesByIdsAsync(companyIds, trackChanges);

        if (companies.Count() != companyIds.Count())
            throw new CollectionCountMismatchBadRequestException();

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public async Task<CompanyDto?> GetCompanyAsync(Guid id, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(id, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(id);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto company, bool trackChanges)
    {
        var companyEntity = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        _mapper.Map(company, companyEntity);

        await _repositoryManager.SaveAsync();
    }
}
