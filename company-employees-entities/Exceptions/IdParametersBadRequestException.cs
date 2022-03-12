namespace company_employees_entities.Exceptions;

public sealed class IdParametersBadRequestException : NotFoundException
{
    public IdParametersBadRequestException()
        : base("Parameter Ids is null or empty")
    {

    }
}
