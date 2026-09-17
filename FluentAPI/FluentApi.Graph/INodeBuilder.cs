namespace FluentApi.Graph;

public interface INodeBuilder : IDotGraphBuilder
{
    IDotGraphBuilder With(Action<INodeAttributes> action);
}
