using System.Dynamic;
using System.Reflection;
using company_employees_service_contracts;

namespace company_employees_services.DataShaping;

public class DataShaper<T> : IDataShaper<T> where T : class
{
    public PropertyInfo[] Properties { get; set; }

    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);

        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject ShapeData(T entity, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);

        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ExpandoObject>();

        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }

    private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject();

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, objectPropertyValue);
        }

        return shapedObject;
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (string.IsNullOrWhiteSpace(fieldString))
            requiredProperties = Properties.ToList();
        else
        {
            var fields = fieldString.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var field in fields)
            {
                var property = Properties.FirstOrDefault(p =>
                    p.Name.Equals(field, StringComparison.InvariantCultureIgnoreCase));
                if (property is null)
                    continue;

                requiredProperties.Add(property);
            }
        }

        return requiredProperties;
    }
}
