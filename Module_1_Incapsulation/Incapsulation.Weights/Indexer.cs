namespace Incapsulation.Weights;

public class Indexer
{
    private readonly double[] _array;
    private readonly int _start;
    private readonly int _length;

    public double this[int index]
    {
        get
        {
            if (index >= _array.Length || index < 0 || index >= _length)
                throw new IndexOutOfRangeException();
            double value = _array[_start + index];
            return value;
        }
        set
        {
            if (index >= _array.Length || index < 0 || index >= _length)
                throw new IndexOutOfRangeException();
            _array[_start + index] = value;
        }
    }

    public Indexer(double[] array, int start, int length)
    {
        if (length < 0 || start < 0 || start + length > array.Length)
            throw new ArgumentException($"Range is invalid with start: {start}, lenth:{length}");
        _array = array;
        _start = start;
        _length = length;
    }

    public int Length => _length;
}