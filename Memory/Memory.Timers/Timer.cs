using System.Diagnostics;

namespace Memory.Timers;

public class Timer : IDisposable
{
    private bool disposedValue;
    private readonly string name;
    private readonly StringWriter writer;
    private readonly Timer? parent;
    private readonly List<Timer> children = new();
    private readonly Stopwatch stopwatch = new();
    private long elapsed;

    public Timer(string name, StringWriter writer, Timer? parent)
    {
        this.name = name;
        this.writer = writer;
        this.parent = parent;
    }

    public static Timer Start(StringWriter writer, string name)
    {
        var timer = new Timer(name, writer, null);
        timer.stopwatch.Start();
        return timer;
    }

    public static Timer Start(StringWriter writer)
    {
        return Start(writer, "*");
    }

    public Timer StartChildTimer(string name)
    {
        var child = new Timer(name, writer, this);
        children.Add(child);
        child.stopwatch.Start();
        return child;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                stopwatch.Stop();
                elapsed = stopwatch.ElapsedMilliseconds;
                if (parent == null)
                {
                    WriteReport(0);
                }
            }
            disposedValue = true;
        }
    }

    ~Timer()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void WriteReport(int level)
    {
        writer.Write(FormatReportLine(name, level, elapsed));
        foreach (var child in children)
        {
            child.WriteReport(level + 1);
        }
        if (children.Count > 0)
        {
            var childrenTime = children.Sum(c => c.elapsed);
            var rest = elapsed - childrenTime;
            writer.Write(FormatReportLine("Rest", level + 1, rest));
        }
    }

    // Use this method in your solution to fit report formatting requirements from the tests
    private static string FormatReportLine(string timerName, int level, long value)
    {
        var intro = new string(' ', level * 4) + timerName;
        return $"{intro,-20}: {value}\n";
    }
}