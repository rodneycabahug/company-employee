using System.ComponentModel.DataAnnotations;

namespace company_employees_shared.DataTransferObjects;

public abstract record EmployeeForManipulationDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Age is required.")]
    [Range(6, int.MaxValue, ErrorMessage = "Age can't be lower than 6")]
    public int Age { get; init; }

    [Required(ErrorMessage = "Position is required.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the position is 20 characters.")]
    public string? Position { get; init; }
}
