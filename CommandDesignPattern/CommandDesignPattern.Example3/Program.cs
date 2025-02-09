
TextEditor textEditor = new();
CommandManager commandManager = new();

ICommand addHelloCommand = new AddTextCommand(textEditor, "Hello ");
ICommand addWorldCommand = new AddTextCommand(textEditor, "World!");

commandManager.ExecuteCommand(addHelloCommand);
commandManager.ExecuteCommand(addWorldCommand);

commandManager.UndoLastCommand();

ICommand deleteCommand = new DeleteTextCommand(textEditor, 6);
commandManager.ExecuteCommand(deleteCommand);

commandManager.UndoLastCommand();

#region Command
public interface ICommand
{
    void Execute();
    void Undo();
}
#endregion

#region Concrete Command
public class AddTextCommand : ICommand
{
    private TextEditor _textEditor;
    private string _text;

    public AddTextCommand(TextEditor textEditor, string text)
    {
        _textEditor = textEditor;
        _text = text;
    }

    public void Execute()
    {
        _textEditor.AddText(_text);
    }

    public void Undo()
    {
        _textEditor.DeleteText(_text.Length);
    }
}

public class DeleteTextCommand : ICommand
{
    private TextEditor _textEditor;
    private int _length;
    private string _deletedText;

    public DeleteTextCommand(TextEditor textEditor, int length)
    {
        _textEditor = textEditor;
        _length = length;
    }

    public void Execute()
    {
        _deletedText = _textEditor.GetText().Substring(_textEditor.GetText().Length - _length);
        _textEditor.DeleteText(_length);
    }

    public void Undo()
    {
        _textEditor.AddText(_deletedText);
    }
}
#endregion

#region Receiver
public class TextEditor
{
    private string _text = "";

    public void AddText(string text)
    {
        _text += text;
        Console.WriteLine($"Text added: {text}");
        Console.WriteLine($"Current Text: {_text}");
    }

    public void DeleteText(int length)
    {
        if (length <= _text.Length)
        {
            string deletedText = _text.Substring(_text.Length - length);
            _text = _text.Remove(_text.Length - length);
            Console.WriteLine($"Text deleted: {deletedText}");
            Console.WriteLine($"Current Text: {_text}");
        }
        else
        {
            Console.WriteLine("Cannot delete. Text is shorter than the requested length.");
        }
    }

    public string GetText()
    {
        return _text;
    }
}
#endregion

#region Invoker
public class CommandManager
{
    private Stack<ICommand> _commandHistory = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
    }

    public void UndoLastCommand()
    {
        if (_commandHistory.Count > 0)
        {
            ICommand lastCommand = _commandHistory.Pop();
            lastCommand.Undo();
        }
        else
        {
            Console.WriteLine("No commands to undo.");
        }
    }
}
#endregion