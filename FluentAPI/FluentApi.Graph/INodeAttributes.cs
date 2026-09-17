namespace FluentApi.Graph;

public interface INodeAttributes
{
    INodeAttributes Color(string color);
    INodeAttributes FontSize(int size);
    INodeAttributes Label(string label);
    INodeAttributes Shape(NodeShape shape);
}
