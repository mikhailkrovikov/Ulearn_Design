using System.Collections;

namespace Generics.BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
{
    public T Value { get; set; }
    public List<T> List { get; set; } = new List<T>();
    public void Add(T value)
    {
        List.Add(value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public BinaryTree<T> Left { get; set; }
    public BinaryTree<T> Right { get; set; }

}

public class BinaryTree : BinaryTree<int>
{
    public static new BinaryTree Create(params int[] values)
    {
        var tree = new BinaryTree();
        foreach (var value in values)
        {
            tree.Add(value);
        }
        return tree;
    }
}
