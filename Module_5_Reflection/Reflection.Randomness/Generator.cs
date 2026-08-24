namespace Reflection.Randomness;

public class FromDistributionAttribute : Attribute
{
    public Type DistributionType { get; }
    public object[] DistributionParameters { get; }
    public FromDistributionAttribute(Type distributionType, params object[] distributionParameters)
    {
        DistributionType = distributionType;
        DistributionParameters = distributionParameters;
    }
}

public class Generator<T>
{
    public T Generate(Random rnd)
    {
        throw new NotImplementedException();
    }
}