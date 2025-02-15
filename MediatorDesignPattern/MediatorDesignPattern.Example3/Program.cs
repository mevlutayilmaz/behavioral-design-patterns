
AirTrafficControl atc = new();

Aircraft passenger1 = new PassengerAircraft(atc, "Airline123");
Aircraft cargo1 = new CargoAircraft(atc, "Cargo456");
Aircraft passenger2 = new PassengerAircraft(atc, "Airline789");

atc.RegisterAircraft(passenger1);
atc.RegisterAircraft(cargo1);
atc.RegisterAircraft(passenger2);

Console.WriteLine("\nAircraft Requests:");
passenger1.RequestLanding();
cargo1.RequestTakeoff();
passenger2.RequestTaxi();

#region Mediator
public interface IAirTrafficControl
{
    void RegisterAircraft(Aircraft aircraft);
    void SendMessage(string message, Aircraft aircraft);
}
#endregion

#region Concrete Mediator
public class AirTrafficControl : IAirTrafficControl
{
    private List<Aircraft> _aircrafts = new List<Aircraft>();

    public void RegisterAircraft(Aircraft aircraft)
    {
        _aircrafts.Add(aircraft);
        Console.WriteLine($"{aircraft.CallSign} registered with Air Traffic Control.");
    }

    public void SendMessage(string message, Aircraft aircraft)
    {
        Console.WriteLine($"Air Traffic Control received message: {message} from {aircraft.CallSign}");

        foreach (var otherAircraft in _aircrafts)
        {
            if (otherAircraft != aircraft)
            {
                otherAircraft.Receive($"ATC: {message} from {aircraft.CallSign}");
            }
        }

        if (message.Contains("requesting landing"))
        {
            Console.WriteLine("Checking for landing conflicts...");
        }
    }
}
#endregion

#region Colleague
public abstract class Aircraft
{
    private IAirTrafficControl _atc;
    public string CallSign { get; set; }

    public Aircraft(IAirTrafficControl atc, string callSign)
    {
        _atc = atc;
        CallSign = callSign;
    }

    public abstract void RequestTaxi();
    public abstract void RequestTakeoff();
    public abstract void RequestLanding();

    protected void Send(string message)
        => _atc.SendMessage(message, this);

    public void Receive(string message)
        => Console.WriteLine($"{CallSign} received message: {message}");
}
#endregion

#region Concrete Colleague
public class PassengerAircraft : Aircraft
{
    public PassengerAircraft(IAirTrafficControl atc, string callSign) : base(atc, callSign)
    {
    }

    public override void RequestTaxi()
    {
        Console.WriteLine($"{CallSign} requesting taxi.");
        Send($"{CallSign} requesting taxi.");
    }

    public override void RequestTakeoff()
    {
        Console.WriteLine($"{CallSign} requesting takeoff.");
        Send($"{CallSign} requesting takeoff.");
    }

    public override void RequestLanding()
    {
        Console.WriteLine($"{CallSign} requesting landing.");
        Send($"{CallSign} requesting landing.");
    }
}

public class CargoAircraft : Aircraft
{
    public CargoAircraft(IAirTrafficControl atc, string callSign) : base(atc, callSign)
    {
    }

    public override void RequestTaxi()
    {
        Console.WriteLine($"{CallSign} requesting taxi.");
        Send($"{CallSign} requesting taxi.");
    }

    public override void RequestTakeoff()
    {
        Console.WriteLine($"{CallSign} requesting takeoff.");
        Send($"{CallSign} requesting takeoff.");
    }

    public override void RequestLanding()
    {
        Console.WriteLine($"{CallSign} requesting landing.");
        Send($"{CallSign} requesting landing.");
    }
}

#endregion