namespace FluentApi.Graph;

public interface IEdgeBuilder : IDotGraphBuilder
{
    IDotGraphBuilder With(Action<IEdgeAttributes> action);
}
