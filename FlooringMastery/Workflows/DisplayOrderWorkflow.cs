using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Workflows
{
    public class DisplayOrderWorkflow
    {
        public void Execute()
        {
            AccountManager manager = AccountManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Display an Order");
            Console.WriteLine($"{Menu.stars}");

            string orderDate;

            // Validates entry and parses to DateTime
            do
            {
                // resets date
                Date.OrderDate = Convert.ToDateTime(Date.DATE_TIME_ORIGIN);

                try
                {
                    Date.OrderDate = DateTime.Parse(ConsoleInput.ConsoleInput.GetStringFromUser(Date.DatePrompt)).Date;
                }
                catch
                {
                    Console.WriteLine("Date must be a legitimate date in the future.");
                }
            } while (Date.OrderDate.Date.ToString() == Date.DATE_TIME_ORIGIN);

            orderDate = Date.OrderDate.ToString();

            // Send response to Data Layer to see if the file exists
            OrderLookupResponse response = manager.DisplayOrder(orderDate);

            if (response.Success)
            {
                ConsoleIO.DisplayOrderListDetails(response.Order);
            }
            else
            {
                Console.WriteLine("An error occurred: ");
                Console.WriteLine(response.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
