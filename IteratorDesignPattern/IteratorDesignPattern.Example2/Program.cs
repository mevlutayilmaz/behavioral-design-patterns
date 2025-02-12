
TreeNode<string> ceo = new("CEO");
TreeNode<string> cto = new("CTO");
TreeNode<string> cfo = new("CFO");

TreeNode<string> devLead = new("Dev Lead");
TreeNode<string> qaLead = new("QA Lead");

TreeNode<string> dev1 = new("Dev 1");
TreeNode<string> dev2 = new("Dev 2");
TreeNode<string> qa1 = new("QA 1");

ceo.Children.Add(cto);
ceo.Children.Add(cfo);

cto.Children.Add(devLead);
cto.Children.Add(qaLead);

devLead.Children.Add(dev1);
devLead.Children.Add(dev2);
qaLead.Children.Add(qa1);

Console.WriteLine("Depth-First Traversal:");
ITreeIterator<string> depthFirstIterator = ceo.CreateIterator(TraversalType.DepthFirst);
while (depthFirstIterator.HasNext())
{
    Console.WriteLine(depthFirstIterator.Next());
}

Console.WriteLine("\nBreadth-First Traversal:");
ITreeIterator<string> breadthFirstIterator = ceo.CreateIterator(TraversalType.BreadthFirst);
while (breadthFirstIterator.HasNext())
{
    Console.WriteLine(breadthFirstIterator.Next());
}

public enum TraversalType
{
    DepthFirst,
    BreadthFirst
}

#region Iterator
public interface ITreeIterator<T>
{
    bool HasNext();
    T Next();
}
#endregion

#region Concrete Iterator
public class DepthFirstIterator<T> : ITreeIterator<T>
{
    private TreeNode<T> _root;
    private Stack<TreeNode<T>> _stack = new();

    public DepthFirstIterator(TreeNode<T> root)
    {
        _root = root;
        _stack.Push(root);
    }

    public bool HasNext()
        => _stack.Count > 0;

    public T Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No more elements.");
        }

        TreeNode<T> node = _stack.Pop();

        for (int i = node.Children.Count - 1; i >= 0; i--)
        {
            _stack.Push(node.Children[i]);
        }

        return node.Data;
    }
}

public class BreadthFirstIterator<T> : ITreeIterator<T>
{
    private TreeNode<T> _root;
    private Queue<TreeNode<T>> _queue = new();

    public BreadthFirstIterator(TreeNode<T> root)
    {
        _root = root;
        _queue.Enqueue(root);
    }

    public bool HasNext()
        => _queue.Count > 0;

    public T Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No more elements.");
        }

        TreeNode<T> node = _queue.Dequeue();

        foreach (var child in node.Children)
        {
            _queue.Enqueue(child);
        }

        return node.Data;
    }
}
#endregion

#region Aggregate
public interface ITree<T>
{
    ITreeIterator<T> CreateIterator(TraversalType traversalType);
}
#endregion

#region Concrete Aggregate
public class TreeNode<T> : ITree<T>
{
    public T Data { get; set; }
    public List<TreeNode<T>> Children { get; set; } = new();

    public TreeNode(T data)
    {
        Data = data;
    }

    public ITreeIterator<T> CreateIterator(TraversalType traversalType)
    {
        switch (traversalType)
        {
            case TraversalType.DepthFirst:
                return new DepthFirstIterator<T>(this);
            case TraversalType.BreadthFirst:
                return new BreadthFirstIterator<T>(this);
            default:
                throw new ArgumentException("Invalid traversal type.");
        }
    }
}
#endregion