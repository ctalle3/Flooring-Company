using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlooringMastery.Workflows
{
    public class AddOrderWorkflow
    {
        public void Execute()
        {
            AccountManager manager = AccountManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Add an Order");
            Console.WriteLine($"{Menu.stars}");

            string orderDate;

            // Validates user input and parses to DateTime
            do
            {
                // resets date
                Date.OrderDate = Convert.ToDateTime(Date.DATE_TIME_ORIGIN);

                try
                {
                    Date.OrderDate = DateTime.Parse(ConsoleInput.ConsoleInput.GetStringFromUser(Date.DatePrompt));
                }
                catch
                {
                    
                    Console.WriteLine("Date must be a legitimate date in the future.");
                }
            } while (Date.OrderDate.Date.ToString() == Date.DATE_TIME_ORIGIN);

            Order order = new Order();

            // Validates Customer Name
            do
            {
                order.CustomerName = ConsoleInput.ConsoleInput.GetStringFromUser("Enter customer's name (a-z 0-9 , or . are excepted): ");

            } while (!(Regex.IsMatch(order.CustomerName, @"^[a-zA-Z0-9., ]+$")));

            // Validates State Abbreviation
            do
            {
                order.State = ConsoleInput.ConsoleInput.GetStringFromUser("Enter the state abbreviation (PA, OH, MI, or IN): ").ToUpper();
            } while (!(order.State == Taxes.statePA || order.State == Taxes.stateOH || 
                       order.State == Taxes.stateMI || order.State == Taxes.stateIN));

            // Validates Product Type
            do
            {
                order.ProductType = ConsoleInput.ConsoleInput.GetStringFromUser("Enter a product ( Carpet, Laminate, Tile, or Wood ): ");
            } while (!(order.ProductType.ToUpper() == Products.typeCarpet || 
                       order.ProductType.ToUpper() == Products.typeLaminate || 
                       order.ProductType.ToUpper() == Products.typeTile || 
                       order.ProductType.ToUpper() == Products.typeWood));

            // Validates Order Area
            order.Area = ConsoleInput.ConsoleInput.GetDecimalFromUser("Enter the area: ", Order.MIN_AREA, EditOrDeleteOrderWorkflow.MAX_INT);

            orderDate = Date.OrderDate.ToString();

            // Prompts you to confirm
            string response;
            do
            {
                ConsoleIO.DisplayAddedOrder(order);
                response = ConsoleInput.ConsoleInput.GetStringFromUser("Would you like to add an order with the above information? (Yes or No) ").ToUpper();
            } while (response != "YES" && response != "NO");

            if(response == "YES")
            {
                // Sends response to Data layer to add an order to an existing file or to create a new file and add it there
                AddEditOrDeleteOrderResponse responseAdd = manager.AddOrder(orderDate, order);

                if (responseAdd.Success)
                {
                    ConsoleIO.DisplayOrderDetails(responseAdd.Order);
                }
                else
                {
                    Console.WriteLine("An error occurred: ");
                    Console.WriteLine(responseAdd.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
