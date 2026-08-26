using System.Reflection;

namespace Reflection.Randomness;

public class FromDistributionAttribute : Attribute
{
    public readonly Type Type;
    public readonly object[] Values;
    public FromDistributionAttribute(Type type, params object[] values)
    {
        Type = type;
        Values = values;
    }
}

public class Generator<T> where T : new()
{
    private readonly Dictionary<PropertyInfo, IContinuousDistribution> dictionary;
    public Generator()
    {
        dictionary = new();
        var props = typeof(T)
            .GetProperties()
            .Where(z => z.GetCustomAttribute<FromDistributionAttribute>() != null);

        foreach (var prop in props)
        {
            try
            {
                var values = prop.GetCustomAttribute<FromDistributionAttribute>().Values;
                var distribution = (IContinuousDistribution)Activator
                    .CreateInstance(prop.GetCustomAttribute<FromDistributionAttribute>().Type, values);
                dictionary.Add(prop, distribution);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }

    public T Generate(Random seed)
    {
        var t = new T();
        foreach (var item in dictionary)
            item.Key.SetValue(t, item.Value.Generate(seed));
        return t;
    }
}