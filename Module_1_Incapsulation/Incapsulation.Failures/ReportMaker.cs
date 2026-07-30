namespace Incapsulation.Failures;

public enum FailureType
{
    [FailureLevel(true)]
    UnexpectedShutdown,

    [FailureLevel(false)]
    ShortNonResponding,

    [FailureLevel(true)]
    HardwareFailures,

    [FailureLevel(false)]
    ConnectionProblems
}

public class Failure
{
    public FailureType Type;

    public Failure(FailureType type)
    {
        Type = type;
    }

    public static bool IsEarlier(DateTime time, DateTime earlier)
    {
        return time < earlier;
    }

    public static bool GetCritical(FailureType type)
    {
        var f = type.GetType().GetField(type.ToString());
        var attr = (FailureLevelAttribute[])f.GetCustomAttributes(
            typeof(FailureLevelAttribute),
            false);
        return attr.FirstOrDefault().IsCritical;
    }
}

public class FailureLevelAttribute : Attribute
{
    public readonly bool IsCritical;
    public FailureLevelAttribute(bool isCritical)
    {
        IsCritical = isCritical;
    }
}

public class Device
{
    public readonly string DeviceName;
    public readonly int DeviceId;
    public Device(string deviceName, int deviceId)
    {
        DeviceName = deviceName;
        DeviceId = deviceId;
    }
}

public class ReportMaker
{
    public static IEnumerable<string> FindDevicesFailedBeforeDate(DateTime time,
        Failure[] failures,
        DateTime[] times,
        List<Device> devices)
    {
        var problematicsDevices = new HashSet<Device>();
        for (int i = 0; i < failures.Length; i++)
            if (Failure.GetCritical(failures[i].Type) && Failure.IsEarlier(times[i], time))
                problematicsDevices.Add(devices[i]);
        return problematicsDevices.Select(d => d.DeviceName);
    }

    /// <summary>
    /// </summary>
    /// <param name="day"></param>
    /// <param name="failureTypes">
    /// 0 for unexpected shutdown, 
    /// 1 for short non-responding, 
    /// 2 for hardware failures, 
    /// 3 for connection problems
    /// </param>
    /// <param name="deviceId"></param>
    /// <param name="times"></param>
    /// <param name="devices"></param>
    /// <returns></returns>
    [Obsolete]
    public static List<string> FindDevicesFailedBeforeDateObsolete(
        int day,
        int month,
        int year,
        int[] failureTypes,
        int[] deviceId,
        object[][] times,
        List<Dictionary<string, object>> devices)
    {
        var dateTime = new DateTime(year, month, day);
        var dateTimes = new DateTime[times.Length];
        for (int i = 0; i < times.Length; i++)
            dateTimes[i] = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
        var newDevices = new List<Device>();
        foreach (var dev in devices)
            newDevices.Add(new Device(dev["Name"].ToString(), (int)dev["DeviceId"]));
        var failures = new Failure[failureTypes.Length];
        for (int i = 0; i < failureTypes.Length; i++)
        {
            failures[i] = failureTypes[i] switch
            {
                0 => new Failure(FailureType.UnexpectedShutdown),
                1 => new Failure(FailureType.ShortNonResponding),
                2 => new Failure(FailureType.HardwareFailures),
                _ => new Failure(FailureType.ConnectionProblems),
            };
        }
        return FindDevicesFailedBeforeDate(dateTime, failures, dateTimes, newDevices).ToList();
    }
}