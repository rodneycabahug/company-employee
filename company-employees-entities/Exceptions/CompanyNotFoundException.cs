namespace company_employees_entities.Exceptions;

public sealed class CompanyNotFoundException: NotFoundException
{
    public CompanyNotFoundException(Guid companyId)
        :base($"The company with id: {companyId} doesn't exist in the database.")
    {

    }
}