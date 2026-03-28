using System;

namespace OneInterfaceManyClasses
{
    // 1 ІНТЕРФЕЙС
    
    public interface IDelivery
    {
        void Deliver(string item);
    }


    
    // БАГАТО КЛАСІВ (реалізацій)
    
    public class CourierDelivery : IDelivery
    {
        public void Deliver(string item)
        {
            Console.WriteLine($"Courier delivered {item}");
        }
    }

    public class PostDelivery : IDelivery
    {
        public void Deliver(string item)
        {
            Console.WriteLine($"Post delivered {item}");
        }
    }

    public class DroneDelivery : IDelivery
    {
        public void Deliver(string item)
        {
            Console.WriteLine($"Drone delivered {item}");
        }
    }

    public class PickupDelivery : IDelivery
    {
        public void Deliver(string item)
        {
            Console.WriteLine($"Customer picked up {item}");
        }
    }


    // ===============================
    // КЛАС, ЯКИЙ НЕ ЗНАЄ РЕАЛІЗАЦІЮ
    // ===============================
    class OrderService
    {
        private readonly IDelivery _delivery;

        public OrderService(IDelivery delivery)
        {
            _delivery = delivery;
        }

        public void SendOrder()
        {
            _delivery.Deliver("Laptop");
        }
    }


    // MAIN
    
    class Program
    {
        static void Main()
        {
            IDelivery delivery;

            delivery = new CourierDelivery();
            new OrderService(delivery).SendOrder();

            delivery = new PostDelivery();
            new OrderService(delivery).SendOrder();

            delivery = new DroneDelivery();
            new OrderService(delivery).SendOrder();

            delivery = new PickupDelivery();
            new OrderService(delivery).SendOrder();
        }
    }
}