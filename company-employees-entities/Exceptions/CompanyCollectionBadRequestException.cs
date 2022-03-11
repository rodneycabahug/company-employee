namespace company_employees_entities.Exceptions;

public sealed class CompanyCollectionBadRequestException: NotFoundException
{
    public CompanyCollectionBadRequestException()
        :base("Company collection is null or empty")
    {

    }
}