using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class AccountManager
    {
        public DateTime DateValidation(string orderDate)
        {
            string errorPrompt = "Date must be a legitimate date in the future.";
            
            //Validates entry and parses to DateTime
            try
            {
                Date.OrderDate = DateTime.Parse(orderDate).Date;
            }
            catch
            {
                Console.WriteLine(errorPrompt);
                Date.OrderDate = Convert.ToDateTime(Date.DATE_TIME_ORIGIN);
            }

            return Date.OrderDate;
        }

        public int IntValidation(string number)
        {
            string errorPrompt = "Order Number must be an integer greater than 1.";
            int orderNumber;

            try
            {
                orderNumber = int.Parse(number);
                if (orderNumber < 1)
                {
                    orderNumber = 0;
                }
            }
            catch
            {
                Console.WriteLine(errorPrompt);
                orderNumber = 0;
            }

            return orderNumber;
        }

        public Order OrderValidation(Order order)
        {
            string errorPrompt = "The Order isn't valid.";
            Order validatedOrder = order;

            if ((order.CustomerName == null) || ((Regex.IsMatch(order.CustomerName, @"^[a-zA-Z0-9., ]+$") == false)))
            {
                Console.WriteLine(errorPrompt);
                validatedOrder = null;
            }
            if (order.State.ToUpper() != Taxes.stateOH && order.State.ToUpper() != Taxes.statePA &&
                order.State.ToUpper() != Taxes.stateMI && order.State.ToUpper() != Taxes.stateIN)
            {
                Console.WriteLine(errorPrompt);
                validatedOrder = null;
            }
            if (order.ProductType.ToUpper() != Products.typeCarpet && 
                order.ProductType.ToUpper() != Products.typeLaminate &&
                order.ProductType.ToUpper() != Products.typeTile && 
                order.ProductType.ToUpper() != Products.typeWood)
            {
                Console.WriteLine(errorPrompt);
                validatedOrder = null;
            }
            if (order.Area < 100)
            {
                Console.WriteLine(errorPrompt);
                validatedOrder = null;
            }

            return validatedOrder;
        }

        private IOrderRepository _orderRepository;

        // DEPENDENCY INJECTION!!!
        public AccountManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository; 
        }

        // sets orderlookup response into motion
        public OrderLookupResponse DisplayOrder(string orderDate)
        {
            OrderLookupResponse response = new OrderLookupResponse();

            DateValidation(orderDate);

            if (Date.OrderDate.ToString() == Date.DATE_TIME_ORIGIN)
            {
                response.Order = null;
            }
            else
            {
                response.Order = _orderRepository.DisplayOrder(Date.OrderDate);
            }

            if (response.Order == null)
            {
                response.Success = false;
                response.Message = $"{orderDate} is not a valid order date.";
            }
            else
            {
                response.Success = true;    
            }

            return response;
        }

        // sets the add edit or delete response into motion for adding an order
        public AddEditOrDeleteOrderResponse AddOrder(string orderDate, Order order)
        {
            AddEditOrDeleteOrderResponse response = new AddEditOrDeleteOrderResponse();

            DateValidation(orderDate);
            Order validatedOrder = OrderValidation(order);

            if ((Date.OrderDate.ToString() == Date.DATE_TIME_ORIGIN) || (validatedOrder == null))
            {
                response.Order = null;
            }
            else
            {
                response.Order = _orderRepository.AddOrder(Date.OrderDate, validatedOrder);
            }

            if (response.Order == null)
            {
                response.Success = false;
                response.Message = "The order could not be added.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        // sets the edit or delete response into motion 
        public AddEditOrDeleteOrderResponse EditOrDeleteOrderSelection(string orderDate, string orderNumber)
        {
            AddEditOrDeleteOrderResponse response = new AddEditOrDeleteOrderResponse();

            DateValidation(orderDate);
            int orderNumberInt = IntValidation(orderNumber);

            if ((Date.OrderDate.ToString() == Date.DATE_TIME_ORIGIN) || (orderNumberInt == 0))
            {
                response.Order = null;
            }
            else
            {
                response.Order = _orderRepository.EditOrDeleteOrderSelection(Date.OrderDate, orderNumberInt);
            }

            if (response.Order == null)
            {
                response.Success = false;
                response.Message = $"{orderDate} or {orderNumber} is not valid";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        // sends edit or delete responses to Data Layer
        public AddEditOrDeleteOrderResponse EditOrDeleteOrder(Order order)
        {
            AddEditOrDeleteOrderResponse response = new AddEditOrDeleteOrderResponse();

            Order validatedOrder = OrderValidation(order);

            if(validatedOrder == null)
            {
                response.Order = null;
            }
            else
            {
                response.Order = _orderRepository.EditOrDeleteOrder(validatedOrder);
            }

            if (response.Order == null)
            {
                response.Success = false;
                response.Message = $"The order wasn't able to be edited.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }
    }
}
