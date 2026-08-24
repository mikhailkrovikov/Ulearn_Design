using System.Text;

namespace Delegates.Observers;

public class StackOperationsLogger
{
    public StringBuilder Log = new();

    public void SubscribeOn<T>(ObservableStack<T> stack)
    {
        stack.StackChanged += HandleEvent;
    }

    public string GetLog()
    {
        return Log.ToString();
    }

    public void HandleEvent(object eventData)
    {
        Log.Append(eventData);
    }
}

public class ObservableStack<T>
{
    public event Action<StackEventData<T>>? StackChanged;

    public void Notify(object eventData)
    {
        StackChanged?.Invoke((StackEventData<T>)eventData);
    }

    private readonly List<T> data = new();

    public void Push(T obj)
    {
        data.Add(obj);
        Notify(new StackEventData<T> { IsPushed = true, Value = obj });
    }

    public T Pop()
    {
        if (data.Count == 0)
            throw new InvalidOperationException();
        var result = data[data.Count - 1];
        Notify(new StackEventData<T> { IsPushed = false, Value = result });
        return result;
    }
}