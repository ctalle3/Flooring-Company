using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlooringMastery.Data
{
    public class TestRepository : IOrderRepository
    {
        // Removes magic numbers
        private const int MAGIC_NUMBER_TWO = 2;
        private const int GET_PERCENT = 100;

        // First order
        public static Order order = new Order
        {
            OrderNumber = 1,
            CustomerName = "Jim",
            Area = 125M,
            MaterialCost = 643.75M,
            LaborCost = 593.75M,
            Tax = (643.75M + 593.75M) * 6.00M / 100M,
            Total = 643.75M + 593.75M + 74.25M,
            State = "IN",
            TaxRate = 6.00M,
            ProductType = "Wood",
            CostPerSquareFoot = 5.15M,
            LaborCostPerSquareFoot = 4.75M,
            DateOrdered = "1/1/2020 12:00:00 AM"
        };

        // Second order
        public static Order order2 = new Order
        {
            OrderNumber = 2,
            CustomerName = "Bill",
            Area = 250M,
            MaterialCost = 1287.50M,
            LaborCost = 1187.50M,
            Tax = (1287.50M + 1187.50M) * 6.00M / 100M,
            Total = 1287.50M + 1187.50M + 148.50M,
            State = "IN",
            TaxRate = 6.00M,
            ProductType = "Wood",
            CostPerSquareFoot = 5.15M,
            LaborCostPerSquareFoot = 4.75M,
            DateOrdered = "1/3/2020 12:00:00 AM"
        };

        // Makes list of orders
        public static List<Order> orderList = new List<Order>() {order , order2};

        public static Taxes taxes = new Taxes
        {
            StateAbbreviation = "IN",
            StateName = "Indiana",
            TaxRate = 6.00M,
        };

        public static Products products = new Products
        {
            ProductType = "Wood",
            CostPerSquareFoot = 5.15M,
            LaborCostPerSquareFoot = 4.75M,
        };

        public List<Order> DisplayOrder(DateTime orderDate)
        {
            // Creates List to be returned
            List<Order> ordersPerDate = new List<Order>();

            //ordersPerDate = null;
            if (Date.OrderDate.ToString() != orderDate.Date.ToString())
            {
                ordersPerDate = null;
            }
            else
            {
                // loops to find the ordrs that have the same date and displays that order
                foreach (var order in orderList)
                {
                    if (order.DateOrdered == orderDate.Date.ToString())
                    {
                        Console.WriteLine($"\n{order.OrderNumber} | {Date.OrderDate}");
                        Console.WriteLine(order.CustomerName);
                        Console.WriteLine(order.State);
                        Console.WriteLine($"Product: {order.ProductType}");
                        Console.WriteLine($"Materials: {order.MaterialCost}");
                        Console.WriteLine($"Labor: {order.LaborCost}");
                        Console.WriteLine($"Tax: {order.Tax}");
                        Console.WriteLine($"Total: {order.Total}\n");
                        ordersPerDate = new List<Order>();
                        break;
                    }
                    else
                    {
                        ordersPerDate = null;
                    }
                }
                if(ordersPerDate != null)
                {
                    ordersPerDate = new List<Order>();
                }
            }
            // returns list
            return ordersPerDate;
        }

        public Order EditOrDeleteOrder(Order order)
        {
            Order editedOrder = null;

            // Checks if order needs to be deleted first
            if (order.CustomerName.Contains("DELETE"))
            {
                // Makes a new list that doesn't include the "DELETED" order
                List<Order> newOrderList = new List<Order>();

                foreach (Order ord in orderList.Where(o => o.OrderNumber == order.OrderNumber))
                {
                    order.CustomerName = $"{order.CustomerName}D";
                }
                foreach(Order ord in orderList.Where(o => o.OrderNumber != order.OrderNumber))
                {
                    newOrderList.Add(ord);
                }

                orderList = newOrderList;
                editedOrder = order;

            }

            // If not deleting then it needs to be edited. Assign new order with passed in order
            foreach (Order ord in orderList.Where(o => o.OrderNumber == order.OrderNumber))
            {
                editedOrder = order;
            }

            return editedOrder;
        }

        public Order EditOrDeleteOrderSelection(DateTime orderDate, int orderNumber)
        {
            Order chosenOrder = null;

            // loops to find order with same orderDate passed in
            foreach (var order in orderList.Where(o => o.DateOrdered == orderDate.ToString()))
            {
                if (order.OrderNumber == orderNumber)
                {
                    chosenOrder = order;
                    break;
                }
                else
                {
                    chosenOrder = null;
                }
            }
            // returns the found order
            return chosenOrder;
        }

        public Order AddOrder(DateTime orderDate, Order order)
        {
            // creates a new order
            Order addedOrder = new Order();

            addedOrder.CustomerName = order.CustomerName;
            addedOrder.ProductType = order.ProductType;
            addedOrder.Area = order.Area;
            addedOrder.State = order.State;
            addedOrder.State = taxes.StateAbbreviation;
            addedOrder.TaxRate = taxes.TaxRate;
            addedOrder.ProductType = products.ProductType;
            addedOrder.CostPerSquareFoot = products.CostPerSquareFoot;
            addedOrder.LaborCostPerSquareFoot = products.LaborCostPerSquareFoot;
            addedOrder.MaterialCost = Math.Round((addedOrder.Area * products.CostPerSquareFoot), MAGIC_NUMBER_TWO);
            addedOrder.LaborCost = Math.Round((addedOrder.Area * products.LaborCostPerSquareFoot), MAGIC_NUMBER_TWO);
            addedOrder.Tax = Math.Round((addedOrder.MaterialCost + addedOrder.LaborCost) * (taxes.TaxRate / GET_PERCENT), MAGIC_NUMBER_TWO);
            addedOrder.Total = Math.Round((addedOrder.MaterialCost + addedOrder.LaborCost + addedOrder.Tax), MAGIC_NUMBER_TWO);
            addedOrder.DateOrdered = orderDate.ToString();

            foreach(var ord in orderList)
            {
                if (ord.DateOrdered == orderDate.Date.ToString())
                {
                    addedOrder.OrderNumber = orderList.Max(o => o.OrderNumber) + 1;
                }
                else
                {
                    addedOrder.OrderNumber = 1;
                    break;
                }
            }

            // adds all the above properties to the new order
            orderList.Add(addedOrder);

            return addedOrder;
        }
    }
}
                                                                                                    