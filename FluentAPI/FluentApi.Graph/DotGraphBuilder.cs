using System.Globalization;

namespace FluentApi.Graph;

public interface IDotGraphBuilder
{
    IEdgeBuilder AddEdge(string sourceNode, string destinationNode);

    INodeBuilder AddNode(string node);

    string Build();
}

public interface IEdgeBuilder : IDotGraphBuilder
{
    IDotGraphBuilder With(Action<IEdgeAttributes> action);
}

public interface INodeBuilder : IDotGraphBuilder
{
    IDotGraphBuilder With(Action<INodeAttributes> action);
}

public interface INodeAttributes
{
    INodeAttributes Color(string color);
    INodeAttributes FontSize(int size);
    INodeAttributes Label(string label);
    INodeAttributes Shape(NodeShape shape);
}

public interface IEdgeAttributes
{
    IEdgeAttributes Color(string color);
    IEdgeAttributes FontSize(int size);
    IEdgeAttributes Label(string label);
    IEdgeAttributes Weight(double weight);
}

public class DotGraphBuilder :
    IDotGraphBuilder,
    IEdgeBuilder,
    INodeBuilder
{
    private Graph _graph;
    private EdgeAttributes _edge;
    private NodeAttributes _node;

    private DotGraphBuilder(Graph graph)
    {
        _graph = graph;
    }

    public static DotGraphBuilder DirectedGraph(string graphName)
    {
        var graph = new Graph(graphName, true, false);
        return new DotGraphBuilder(graph);
    }

    public static DotGraphBuilder UndirectedGraph(string graphName)
    {
        var graph = new Graph(graphName, false, false);
        return new DotGraphBuilder(graph);
    }

    public IEdgeBuilder AddEdge(string sourceNode, string destinationNode)
    {
        _edge = new EdgeAttributes(_graph.AddEdge(sourceNode, destinationNode));
        return this;
    }

    public INodeBuilder AddNode(string node)
    {
        _node = new NodeAttributes(_graph.AddNode(node));
        return this;
    }

    public string Build()
    {
        return _graph.ToDotFormat();
    }


    IDotGraphBuilder IEdgeBuilder.With(Action<IEdgeAttributes> action)
    {
        action(_edge);
        return this;
    }

    IDotGraphBuilder INodeBuilder.With(Action<INodeAttributes> action)
    {
        action(_node);
        return this;
    }
}

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

public enum NodeShape
{
    Box,
    Ellipse
}