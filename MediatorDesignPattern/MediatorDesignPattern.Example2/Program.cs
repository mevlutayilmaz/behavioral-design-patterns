
IChatMediator chatMediator = new ChatMediator();

User user1 = new NormalUser(chatMediator, "Alice");
User user2 = new NormalUser(chatMediator, "Bob");
User admin = new AdminUser(chatMediator, "Admin");

chatMediator.AddUser(user1);
chatMediator.AddUser(user2);
chatMediator.AddUser(admin);

user1.Send("Hi everyone!");
user2.Send("Hello Alice!");
admin.Send("Welcome to the chat!");

#region Mediator
public interface IChatMediator
{
    void SendMessage(string message, User user);
    void AddUser(User user);
}
#endregion

#region Concrete Mediator
public class ChatMediator : IChatMediator
{
    private List<User> _users = new();

    public void AddUser(User user)
        => _users.Add(user);

    public void SendMessage(string message, User sender)
    {
        foreach (var user in _users)
        {
            if (user != sender)
            {
                user.Receive(message);
            }
        }
    }
}
#endregion

#region Colleague
public abstract class User
{
    protected IChatMediator _mediator;
    public string Name { get; }

    public User(IChatMediator mediator, string name)
    {
        _mediator = mediator;
        Name = name;
    }

    public abstract void Send(string message);
    public abstract void Receive(string message);
}
#endregion

#region Concrete Colleague
public class NormalUser : User
{
    public NormalUser(IChatMediator mediator, string name) : base(mediator, name) { }

    public override void Send(string message)
    {
        Console.WriteLine($"{Name} sends: {message}");
        _mediator.SendMessage(message, this);
    }

    public override void Receive(string message)
        => Console.WriteLine($"{Name} receives: {message}");
}

public class AdminUser : User
{
    public AdminUser(IChatMediator mediator, string name) : base(mediator, name) { }

    public override void Send(string message)
    {
        Console.WriteLine($"{Name} (Admin) sends: {message}");
        _mediator.SendMessage(message, this);
    }

    public override void Receive(string message)
        => Console.WriteLine($"{Name} (Admin) receives: {message}");
}
#endregion