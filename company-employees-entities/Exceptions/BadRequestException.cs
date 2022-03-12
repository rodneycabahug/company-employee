namespace company_employees_entities.Exceptions;

public class BadRequestException : Exception
{
    protected BadRequestException(string message) : base(message) { }
}
