using System.Reflection;
using System.Text;

namespace company_employees_repositories.Extensions.Utility;

public static class OrderQueryBuilder
{
    public static string CreateOrderQuery<T>(string orderByQueryString)
    {
        var orderParams = orderByQueryString.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderByQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            var propertyFromQueryString = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryString, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty is null)
                continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";

            orderByQueryBuilder.Append($"{objectProperty.Name} {direction}");
        }

        var orderQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' ');

        return orderQuery;
    }
}
