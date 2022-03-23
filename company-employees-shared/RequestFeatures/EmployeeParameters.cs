namespace company_employees_shared.RequestFeatures;

public class EmployeeParameters : RequestParameters
{
    public int MinAge { get; set; } = 0;
    public int MaxAge { get; set; } = int.MaxValue;

    public bool ValidAgeRange => MaxAge >= MinAge;
}
