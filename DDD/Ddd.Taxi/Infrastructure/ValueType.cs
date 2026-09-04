using Ddd.Taxi.Domain;
using NUnit.Framework;
using System.Reflection;

namespace Ddd.Taxi.Infrastructure;

/// <summary>
/// Базовый класс для всех Value типов.
/// </summary>
public abstract class ValueType<T>
{
    private readonly PropertyInfo[] _properties;

    protected ValueType()
    {
        _properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(p => p.Name)
            .ToArray();
    }

    public override string ToString()
    {
        return $"{GetType().Name}(" + string.Join("; ", _properties.Select(p => $"{p.Name}: {p.GetValue(this)}")) + ")";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, this))
            return true;

        if (obj is null || obj.GetType() != GetType())
            return false;

        var thisValues = _properties
            .Select(p => p.GetValue(this));

        var otherValues = _properties
            .Select(p => p.GetValue(obj));

        return thisValues.SequenceEqual(otherValues);
    }

    public bool Equals(T? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var property in _properties)
            hash.Add(property.GetValue(this));
        return hash.ToHashCode();
    }
}