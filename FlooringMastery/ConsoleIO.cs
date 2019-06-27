using FlooringMastery.Models;
using FlooringMastery.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery
{
    public class ConsoleIO
    {
        public static void DisplayOrderListDetails(List<Order> orders)
        {
            foreach(var o in orders)
            {
                Console.WriteLine($"\n{o.OrderNumber} | {Date.OrderDate}");
                Console.WriteLine(o.CustomerName);
                Console.WriteLine(o.State);
                Console.WriteLine($"Product: {o.ProductType}");
                Console.WriteLine($"Materials: {o.MaterialCost}");
                Console.WriteLine($"Labor: {o.LaborCost}");
                Console.WriteLine($"Tax: {o.Tax}");
                Console.WriteLine($"Total: {o.Total}\n");
            }
        }

        public static void DisplayAddedOrder(Order order)
        {
            Console.Clear();
            Console.WriteLine($"Customer Name: {order.CustomerName}");
            Console.WriteLine($"State: {order.State}");
            Console.WriteLine($"Product: {order.ProductType}");
            Console.WriteLine($"Area: order.Area\n");
        }

        public static void DisplayOrderDetails(Order order)
        {
            Console.Clear();

            if (order.CustomerName.Contains("DELETED"))
            {
                Console.WriteLine("\nThe below order was deleted.");
                order.CustomerName = order.CustomerName.Remove(order.CustomerName.Count() - 8);
            }

            if (order.CustomerName.Contains(""))
            {
                order.CustomerName = order.CustomerName.Replace("\"", "");
            }

            Console.WriteLine($"\n{order.OrderNumber} | {Date.OrderDate}");
            Console.WriteLine(order.CustomerName);
            Console.WriteLine(order.State);
            Console.WriteLine($"Product: {order.ProductType}");
            Console.WriteLine($"Materials: {order.MaterialCost}");
            Console.WriteLine($"Labor: {order.LaborCost}");
            Console.WriteLine($"Tax: {order.Tax}");
            Console.WriteLine($"Total: {order.Total}\n");
        }
    }
}
