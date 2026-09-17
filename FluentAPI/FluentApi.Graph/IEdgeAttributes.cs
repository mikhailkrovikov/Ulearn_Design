namespace FluentApi.Graph;

public interface IEdgeAttributes
{
    IEdgeAttributes Color(string color);
    IEdgeAttributes FontSize(int size);
    IEdgeAttributes Label(string label);
    IEdgeAttributes Weight(double weight);
}
