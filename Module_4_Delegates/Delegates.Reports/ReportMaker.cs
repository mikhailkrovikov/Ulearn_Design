using System.Text;

namespace Delegates.Reports;

public class ReportMaker
{
    private readonly Func<string, string> makeCaption;
    private readonly Func<string> beginList;
    private readonly Func<string, string, string> makeItem;
    private readonly Func<string> endList;
    private readonly Func<IEnumerable<double>, object> makeStatistics;
    private readonly Func<string> caption;

    public ReportMaker(
        Func<string, string> makeCaption,
        Func<string> beginList,
        Func<string, string, string> makeItem,
        Func<string> endList,
        Func<IEnumerable<double>, object> makeStatistics,
        Func<string> caption)
    {
        this.makeCaption = makeCaption;
        this.beginList = beginList;
        this.makeItem = makeItem;
        this.endList = endList;
        this.makeStatistics = makeStatistics;
        this.caption = caption;
    }

    public string MakeReport(IEnumerable<Measurement> measurements)
    {
        var data = measurements.ToList();
        var result = new StringBuilder();
        result.Append(makeCaption(caption()));
        result.Append(beginList());
        result.Append(makeItem("Temperature", makeStatistics(data.Select(z => z.Temperature)).ToString()));
        result.Append(makeItem("Humidity", makeStatistics(data.Select(z => z.Humidity)).ToString()));
        result.Append(endList());
        return result.ToString();
    }
}

public static class ReportMakerHelper
{
    public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
    {
        var report = new ReportMaker(
            (s) => $"<h1>{s}</h1>",
            () => "<ul>",
            (valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
            () => "</ul>",
            (data) =>
            {
                var list = data.ToList();
                var mean = list.Average();
                var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));
                return new MeanAndStd
                {
                    Mean = mean,
                    Std = std
                };
            },
            () => "Mean and Std"
            );
        return report.MakeReport(data);
    }

    public static string MedianMarkdownReport(IEnumerable<Measurement> data)
    {
        var report = new ReportMaker(
            (s) => $"## {s}\n\n",
            () => "",
            (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
            () => "",
            (data) =>
            {
                var list = data.OrderBy(z => z).ToList();
                if (list.Count % 2 == 0)
                    return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
                return list[list.Count / 2];
            },
            () => "Median"
            );
        return report.MakeReport(data);
    }

    public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
    {
        var report = new ReportMaker(
            (s) => $"## {s}\n\n",
            () => "",
            (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
            () => "",
            (data) =>
            {
                var list = data.ToList();
                var mean = list.Average();
                var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));
                return new MeanAndStd
                {
                    Mean = mean,
                    Std = std
                };
            },
            () => "Mean and Std"
            );
        return report.MakeReport(measurements);
    }

    public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
    {
        var report = new ReportMaker(
            (s) => $"<h1>{s}</h1>",
            () => "<ul>",
            (valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
            () => "</ul>",
            (data) =>
            {
                var list = data.OrderBy(z => z).ToList();
                if (list.Count % 2 == 0)
                    return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
                return list[list.Count / 2];
            },
            () => "Median"
            );
        return report.MakeReport(measurements);
    }
}