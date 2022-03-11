namespace company_employees_entities.Exceptions;

public class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message) {}
}