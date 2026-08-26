namespace Delegates.TreeTraversal;

public static class Traversal
{
    public static IEnumerable<TResult> TraversalTree<TNode, TResult>(
        TNode node,
        Func<TNode, IEnumerable<TNode>> getChildren,
        Func<TNode, IEnumerable<TResult>> getValues)
    {
        foreach (var value in getValues(node))
            yield return value;
        foreach (var child in getChildren(node))
            foreach (var value in TraversalTree(child, getChildren, getValues))
                yield return value;
    }

    public static IEnumerable<Product> GetProducts(ProductCategory root)
    {
        return TraversalTree(root,
            p => p.Categories,
            p => p.Products);
    }

    public static IEnumerable<Job> GetEndJobs(Job root)
    {
        return TraversalTree(root,
            j => j.Subjobs,
            j => j.Subjobs.Count == 0 ? [j] : Array.Empty<Job>());
    }

    public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
    {
        return TraversalTree(root,
            n => new[] { n.Left, n.Right }.Where(c => c != null)!,
            n => n.Left == null && n.Right == null
            ? [n.Value]
            : Array.Empty<T>());
    }
}