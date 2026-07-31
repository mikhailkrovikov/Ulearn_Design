namespace Inheritance.DataStructure;

public class Category : IComparable
{
    public readonly string Product;
    public readonly MessageType Type;
    public readonly MessageTopic Topic;

    public Category(string text, MessageType type, MessageTopic topic)
    {
        Product = text;
        Type = type;
        Topic = topic;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Category other)
            return false;
        return this.GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Product, Type, Topic);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Category)
            return -1;
        var other = obj as Category;
        return (Product, Type, Topic)
            .CompareTo((other.Product, other.Type, other.Topic));
    }

    public override string ToString()
    {
        return Product + "." + Type.ToString() + "." + Topic.ToString();
    }

    public static bool operator !=(Category category, Category other)
    {
        return !category.Equals(other);
    }

    public static bool operator ==(Category category, Category other)
    {
        return category.Equals(other);
    }

    public static bool operator >(Category category, Category other)
    {
        return category.CompareTo(other) > 0;
    }

    public static bool operator <(Category category, Category other)
    {
        return category.CompareTo(other) < 0;
    }

    public static bool operator >=(Category category, Category other)
    {
        return category.CompareTo(other) >= 0;
    }

    public static bool operator <=(Category category, Category other)
    {
        return category.CompareTo(other) <= 0;
    }
}