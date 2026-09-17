using System.Globalization;

namespace FluentApi.Graph;

public class NodeAttributes : INodeAttributes
{
    private readonly GraphNode _node;
    public NodeAttributes(GraphNode node)
    {
        _node = node;
    }

    public INodeAttributes Color(string color)
    {
        _node.Attributes.TryAdd("color", color);
        return this;
    }

    public INodeAttributes FontSize(int size)
    {
        _node.Attributes.TryAdd("fontsize", size.ToString(CultureInfo.InvariantCulture));
        return this;
    }

    public INodeAttributes Label(string label)
    {
        _node.Attributes.TryAdd("label", label);
        return this;
    }

    public INodeAttributes Shape(NodeShape shape)
    {
        _node.Attributes.TryAdd("shape", shape.ToString().ToLower());
        return this;
    }
}
