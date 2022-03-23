namespace company_employees_entities.Exceptions;

public class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException() :
        base("Max age can't be less than min age.")
    { }
}
