using System.ComponentModel.DataAnnotations;

namespace company_employees_shared.DataTransferObjects;

public record CompanyForUpdateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length for name is 50 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Address is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length for address is 50 characters.")]
    public string? Address { get; init; }

    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length for country is 50 characters.")]
    public string? Country { get; init; }
    public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
}
