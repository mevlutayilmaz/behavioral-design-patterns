
OrderHandler validation = new ValidationHandler();
OrderHandler discount = new DiscountHandler();
OrderHandler payment = new PaymentHandler();
OrderHandler shipping = new ShippingHandler();

validation.SetNext(discount);
discount.SetNext(payment);
payment.SetNext(shipping);

Order order = new();
validation.Process(order);

public class Order
{
    public bool IsValid { get; set; } = true;

    public void ApplyDiscount()
        => Console.WriteLine("Discount Applied...");

    public bool ProcessPayment()
    {
        Console.WriteLine("Payment Processed...");
        return true;
    }

    public void Ship()
        => Console.WriteLine("Order Shipped...");
}

#region Handler
public abstract class OrderHandler
{
    protected OrderHandler _nextHandler;
    public void SetNext(OrderHandler handler)
    {
        _nextHandler = handler;
    }
    public abstract void Process(Order order);
}
#endregion

#region Concrete Handler
public class ValidationHandler : OrderHandler
{
    public override void Process(Order order)
    {
        if (order.IsValid)
        {
            Console.WriteLine("Order validation passed.");
            if (_nextHandler != null) _nextHandler.Process(order);
        }
        else
        {
            Console.WriteLine("Order validation failed. Halting process.");
        }
    }
}

public class DiscountHandler : OrderHandler
{
    public override void Process(Order order)
    {
        order.ApplyDiscount();
        Console.WriteLine("Discount applied to order if any.");
        if (_nextHandler != null) _nextHandler.Process(order);
    }
}
public class PaymentHandler : OrderHandler
{
    public override void Process(Order order)
    {
        if (order.ProcessPayment())
        {
            Console.WriteLine("Payment processed successfully.");
            if (_nextHandler != null) _nextHandler.Process(order);
        }
        else
        {
            Console.WriteLine("Payment processing failed. Halting process.");
        }
    }
}
public class ShippingHandler : OrderHandler
{
    public override void Process(Order order)
    {
        order.Ship();
        Console.WriteLine("Order shipped to customer.");
    }
}
#endregion