namespace company_employees_shared.DataTransferObjects;

public record CompanyDto
{
    public Guid Id { get; init; }
    public string?  Name { get; init; }
    public string? FullAddress { get; init; }
}
