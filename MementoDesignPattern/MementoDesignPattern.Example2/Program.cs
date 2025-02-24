
TextEditor editor = new();
History history = new(editor);

editor.Content = "First line";
history.Save();

editor.Content = "Second line";
history.Save();

editor.Content = "Third line";
history.Save();

history.Undo();
history.Undo();
history.Undo();


#region Memento
public class TextMemento
{
    public string Content { get; set; }
    public TextMemento(string content)
    {
        Content = content;
    }
}
#endregion

#region Originator
public class TextEditor
{
    public string Content { get; set; }

    public TextMemento Save()
        => new TextMemento(Content);

    public void Restore(TextMemento memento)
        => Content = memento.Content;
}
#endregion

#region Caretaker
public class History
{
    private Stack<TextMemento> _mementos = new();
    private TextEditor _textEditor;

    public History(TextEditor textEditor)
    {
        _textEditor = textEditor;
    }

    public void Save()
        => _mementos.Push(_textEditor.Save());

    public void Undo()
    {
        if (_mementos.Count > 0)
        {
            _textEditor.Restore(_mementos.Pop());
        }
        else
        {
            Console.WriteLine("No more states to undo.");
        }
    }
}
#endregion