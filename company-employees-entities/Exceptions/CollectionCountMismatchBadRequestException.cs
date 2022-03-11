namespace company_employees_entities.Exceptions;

public sealed class CollectionCountMismatchBadRequestException: NotFoundException
{
    public CollectionCountMismatchBadRequestException()
        :base("Collection count mismatch compaiting to parameter Ids")
    {

    }
}