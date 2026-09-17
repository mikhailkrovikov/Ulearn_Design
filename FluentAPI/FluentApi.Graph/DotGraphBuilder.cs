namespace FluentApi.Graph;

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