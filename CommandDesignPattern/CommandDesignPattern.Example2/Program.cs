
Light light = new();
AirConditioner ac = new();

ICommand lightOn = new LightOnCommand(light);
ICommand lightOff = new LightOffCommand(light);
ICommand acOn = new AirConditionerOnCommand(ac);
ICommand acOff = new AirConditionerOffCommand(ac);

RemoteControl remote = new RemoteControl();

// Işığı aç, kapat ve geri al
Console.WriteLine("▶ Controlling Light");
remote.SetCommand(lightOn);
remote.PressButton();
remote.PressUndo();

remote.SetCommand(lightOff);
remote.PressButton();
remote.PressUndo();

Console.WriteLine("\n▶ Controlling Air Conditioner");
remote.SetCommand(acOn);
remote.PressButton();
remote.PressUndo();

remote.SetCommand(acOff);
remote.PressButton();
remote.PressUndo();

#region Command
public interface ICommand
{
    void Execute();
    void Undo();
}
#endregion

#region Concrete Command
public class LightOnCommand : ICommand
{
    private Light _light;

    public LightOnCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
        => _light.TurnOn();

    public void Undo()
        => _light.TurnOff();
}

public class LightOffCommand : ICommand
{
    private Light _light;

    public LightOffCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
        => _light.TurnOff();

    public void Undo()
        => _light.TurnOn();
}

public class AirConditionerOnCommand : ICommand
{
    private AirConditioner _ac;

    public AirConditionerOnCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute()
        => _ac.TurnOn();

    public void Undo()
        => _ac.TurnOff();
}

public class AirConditionerOffCommand : ICommand
{
    private AirConditioner _ac;

    public AirConditionerOffCommand(AirConditioner ac)
    {
        _ac = ac;
    }

    public void Execute()
        => _ac.TurnOff();

    public void Undo()
        => _ac.TurnOn();
}
#endregion

#region Receiver
public class Light
{
    public void TurnOn()
        => Console.WriteLine("💡 Light is ON.");

    public void TurnOff()
        => Console.WriteLine("🔌 Light is OFF.");
}

public class AirConditioner
{
    public void TurnOn()
        => Console.WriteLine("❄️ Air Conditioner is ON.");

    public void TurnOff()
        => Console.WriteLine("🔥 Air Conditioner is OFF.");
}
#endregion

#region Invoker
public class RemoteControl
{
    private ICommand _command;
    private Stack<ICommand> _history = new Stack<ICommand>();

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void PressButton()
    {
        _command.Execute();
        _history.Push(_command);
    }

    public void PressUndo()
    {
        if (_history.Count > 0)
        {
            ICommand lastCommand = _history.Pop();
            lastCommand.Undo();
        }
    }
}
#endregion