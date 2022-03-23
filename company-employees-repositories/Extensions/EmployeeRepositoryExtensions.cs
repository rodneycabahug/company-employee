using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using company_employees_entities.Models;
using company_employees_repositories.Extensions.Utility;

namespace company_employees_repositories.Extensions;

public static class EmployeeRepositoryExtensions
{
    public static IQueryable<Employee> Filter(
        this IQueryable<Employee> employees, uint minAge, uint maxAge)
    {
        return employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
    }

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;

        var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

        return employees.Where(e => e.Name.ToLower().Contains(lowerCaseSearchTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.Name);

        return employees.OrderBy(orderQuery);
    }
}
