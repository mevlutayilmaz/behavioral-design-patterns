
Account account = new("MyAccount123", 1000);

AccountHandler balanceCheck = new BalanceCheckHandler();
AccountHandler limitCheck = new LimitCheckHandler();
AccountHandler blacklistCheck = new BlacklistCheckHandler();
AccountHandler transactionLog = new TransactionLogHandler();

balanceCheck.SetNext(limitCheck).SetNext(blacklistCheck).SetNext(transactionLog);

Console.WriteLine("Attempting transaction of 200...");
balanceCheck.HandleRequest(account, 200);

Console.WriteLine("\nAttempting transaction of 6000...");
balanceCheck.HandleRequest(account, 6000);

Console.WriteLine("\nAttempting transaction of 50...");
balanceCheck.HandleRequest(account, 50);

Account blacklistedAccount = new("BlockedAccount1", 500);
Console.WriteLine("\nAttempting transaction of 100 for blacklisted account...");
balanceCheck.HandleRequest(blacklistedAccount, 100);

public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public Account(string accountNumber, decimal balance)
    {
        AccountNumber = accountNumber;
        Balance = balance;
    }
}

#region Handler
public abstract class AccountHandler
{
    private AccountHandler _nextHandler;

    public AccountHandler SetNext(AccountHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }

    public abstract void HandleRequest(Account account, decimal amount);

    protected void HandleNext(Account account, decimal amount)
    {
        if (_nextHandler != null)
        {
            _nextHandler.HandleRequest(account, amount);
        }
        else
        {
            Console.WriteLine("Transaction completed successfully.");
        }
    }
}
#endregion

#region Concrete Handler
public class BalanceCheckHandler : AccountHandler
{
    public override void HandleRequest(Account account, decimal amount)
    {
        Console.WriteLine("Checking account balance...");
        if (account.Balance < amount)
        {
            Console.WriteLine("Insufficient balance.");
            return;
        }

        Console.WriteLine("Balance check passed.");
        HandleNext(account, amount);
    }
}

public class LimitCheckHandler : AccountHandler
{
    private decimal _dailyLimit = 5000;

    public override void HandleRequest(Account account, decimal amount)
    {
        Console.WriteLine("Checking transaction limit...");
        if (amount > _dailyLimit)
        {
            Console.WriteLine($"Transaction exceeds daily limit of {_dailyLimit}.");
            return;
        }

        Console.WriteLine("Limit check passed.");
        HandleNext(account, amount);
    }
}

public class BlacklistCheckHandler : AccountHandler
{
    private List<string> _blacklistedAccounts = new List<string> { "BlockedAccount1", "BlockedAccount2" };

    public override void HandleRequest(Account account, decimal amount)
    {
        Console.WriteLine("Checking blacklist...");
        if (_blacklistedAccounts.Contains(account.AccountNumber))
        {
            Console.WriteLine("Account is blacklisted. Transaction blocked.");
            return;
        }

        Console.WriteLine("Blacklist check passed.");
        HandleNext(account, amount);
    }
}

public class TransactionLogHandler : AccountHandler
{
    public override void HandleRequest(Account account, decimal amount)
    {
        Console.WriteLine("Logging transaction...");
        Console.WriteLine($"Transaction of {amount} processed for account {account.AccountNumber}.");

        account.Balance -= amount;
        Console.WriteLine($"New balance: {account.Balance}");

        HandleNext(account, amount);
    }
}
#endregion