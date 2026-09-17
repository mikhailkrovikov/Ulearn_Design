using System.Globalization;

namespace FluentApi.Graph;


public class DotGraphBuilder
{
    private Graph graph;

    public DotGraphBuilder(Graph graph)
    {
        this.graph = graph;
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

    public NodeBuilder AddNode(string node)
    {
        var nodeBuilder = new NodeBuilder(graph);
        nodeBuilder.Add(node);
        return nodeBuilder;
    }

    public EdgeBuilder AddEdge(string sourceNode, string destinationNode)
    {
        var edgeBuilder = new EdgeBuilder(graph);
        edgeBuilder.Add(sourceNode, destinationNode);
        return edgeBuilder;
    }

    public string Build()
    {
        return graph.ToDotFormat();
    }
}

public class EdgeBuilder : DotGraphBuilder
{
    private Graph graph;
    private GraphEdge edge;
    public EdgeBuilder(Graph graph) : base(graph)
    {
        this.graph = graph;
    }
    public EdgeBuilder Add(string sourceNode, string destinationNode)
    {
        edge = new GraphEdge(sourceNode, destinationNode, graph.Directed);
        graph.AddEdge(sourceNode, destinationNode);
        return this;
    }

    public EdgeBuilder With(Action<GraphEdge> action)
    {
        action(edge);
        return this;
    }
}
public class NodeBuilder : DotGraphBuilder
{
    private Graph graph;
    private GraphNode _node;
    public NodeBuilder(Graph graph) : base(graph)
    {
        this.graph = graph;
    }
    public NodeBuilder Add(string node)
    {
        _node = new GraphNode(node);
        graph.AddNode(node);
        return this;
    }
    public NodeBuilder With(Action<GraphNode> action)
    {
        action(_node);
        return this;
    }
}

public enum NodeShape
{
    Box,
    Ellipse
}

public static class GraphNodeExtensions
{
    public static GraphNode Color(this GraphNode node, string color)
    {
        node.Attributes.TryAdd("color", color);
        return node;
    }

    public static GraphNode FontSize(this GraphNode node, int size)
    {
        node.Attributes.TryAdd("fontsize", size.ToString(CultureInfo.InvariantCulture));
        return node;
    }

    public static GraphNode Label(this GraphNode node, string label)
    {
        node.Attributes.TryAdd("label", label);
        return node;
    }

    public static GraphNode Shape(this GraphNode node, NodeShape shape)
    {
        node.Attributes.TryAdd("shape", shape.ToString().ToLower());
        return node;
    }
}

public static class GraphEdgeExtensions
{
    public static GraphEdge Color(this GraphEdge edge, string color)
    {
        edge.Attributes.TryAdd("color", color);
        return edge;
    }

    public static GraphEdge FontSize(this GraphEdge edge, int size)
    {
        edge.Attributes.TryAdd("fontsize", size.ToString(CultureInfo.InvariantCulture));
        return edge;
    }

    public static GraphEdge Label(this GraphEdge edge, string label)
    {
        edge.Attributes.TryAdd("label", label);
        return edge;
    }

    public static GraphEdge Weight(this GraphEdge edge, double weight)
    {
        edge.Attributes.TryAdd("weight", weight.ToString(CultureInfo.InvariantCulture));
        return edge;
    }
}