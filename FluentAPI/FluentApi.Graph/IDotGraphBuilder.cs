namespace FluentApi.Graph;

public interface IDotGraphBuilder
{
    IEdgeBuilder AddEdge(string sourceNode, string destinationNode);

    INodeBuilder AddNode(string node);

    string Build();
}
