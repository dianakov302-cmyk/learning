using System;
// Одна реалізація - багато раеалізацій (One Interface - Many Classes)
public interface IPayment
{
    void Pay(decimal amount);
}
public class CreditCardPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using Credit Card");
    }
}
public class PayPalPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using PayPal");
    }       
}
public class BankTransferPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using Bank Transfer");
    }
}
public class CashPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using Cash");
    }
}   
class Checkout
{
    private readonly IPayment _payment;

    public Checkout(IPayment payment)
    {
        _payment = payment;
    }

    public void ProcessPayment(decimal amount)
    {
        _payment.Pay(amount);
    }
}
// 2 приклад
