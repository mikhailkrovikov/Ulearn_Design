namespace Delegates.PairsAnalysis;

public static class Analysis
{
    public static int FindMaxPeriodIndex(params DateTime[] data)
    {
        return data.Pairs()
            .Select(p => (p.Item2 - p.Item1).TotalSeconds)
            .MaxIndex();
    }

    public static double FindAverageRelativeDifference(params double[] data)
    {
        return data.Pairs()
            .Average(x => (x.Item2 - x.Item1) / x.Item1);
    }
}

public static class MyExtentions
{
    public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> collection)
    {
        using var e = collection.GetEnumerator();
        if (!e.MoveNext())
            yield break;
        var current = e.Current;
        while (e.MoveNext())
        {
            var next = e.Current;
            yield return Tuple.Create(current, next);
            current = next;
        }
    }

    public static int MaxIndex<T>(this IEnumerable<T> collection) where T : IComparable<T>
    {
        var maxValue = default(T);
        var maxIndex = -1;
        var i = 0;
        foreach (var item in collection)
        {
            if (item.CompareTo(maxValue) >= 0)
            {
                maxValue = item;
                maxIndex = i;
            }
            i++;
        }
        if (maxIndex == -1)
            throw new InvalidOperationException();
        return maxIndex;
    }
}