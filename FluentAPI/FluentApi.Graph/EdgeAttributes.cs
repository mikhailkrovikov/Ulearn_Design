using System.Globalization;

namespace FluentApi.Graph;

public class EdgeAttributes : IEdgeAttributes
{
    private readonly GraphEdge _edge;
    public EdgeAttributes(GraphEdge edge)
    {
        _edge = edge;
    }

    public IEdgeAttributes Color(string color)
    {
        _edge.Attributes.TryAdd("color", color);
        return this;
    }

    public IEdgeAttributes FontSize(int size)
    {
        _edge.Attributes.TryAdd("fontsize", size.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public IEdgeAttributes Label(string label)
    {
        _edge.Attributes.TryAdd("label", label);
        return this;
    }

    public IEdgeAttributes Weight(double weight)
    {
        _edge.Attributes.TryAdd("weight", weight.ToString(CultureInfo.InvariantCulture));
        return this;
    }
}
