using System.Collections;

WordsCollection collection = new();
collection.AddItem("First");
collection.AddItem("Second");
collection.AddItem("Third");

Console.WriteLine("Straight traversal:");

foreach (var element in collection)
{
    Console.WriteLine(element);
}

Console.WriteLine("\nReverse traversal:");

collection.ReverseDirection();

foreach (var element in collection)
{
    Console.WriteLine(element);
}

public abstract class Iterator : IEnumerator
{
    object IEnumerator.Current => Current();

    public abstract int Key();
    public abstract object Current();
    public abstract bool MoveNext();
    public abstract void Reset();
}

public abstract class IteratorAggregate : IEnumerable
{
    public abstract IEnumerator GetEnumerator();
}

public class AlphabeticalOrderIterator : Iterator
{
    private WordsCollection _collection;
    private int _position = -1;
    private bool _reverse = false;

    public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
    {
        _collection = collection;
        _reverse = reverse;

        if (reverse)
            _position = collection.getItems().Count;
    }

    public override object Current()
        => _collection.getItems()[_position];

    public override int Key()
        => _position;

    public override bool MoveNext()
    {
        int updatedPosition = _position + (_reverse ? -1 : 1);

        if (updatedPosition >= 0 && updatedPosition < _collection.getItems().Count)
        {
            _position = updatedPosition;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Reset()
        => _position = _reverse ? _collection.getItems().Count - 1 : 0;
}

public class WordsCollection : IteratorAggregate
{
    List<string> _collection = new List<string>();
    bool _direction = false;

    public void ReverseDirection()
        => _direction = !_direction;

    public List<string> getItems()
        => _collection;

    public void AddItem(string item)
        => _collection.Add(item);

    public override IEnumerator GetEnumerator()
        => new AlphabeticalOrderIterator(this, _direction);
}