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
    public class EditOrDeleteOrderWorkflow
    {
        public const int MAX_INT = 2000000000;

        public const string DATE_TIME_ORIGIN = "1/1/0001 12:00:00 AM";
        private int _fiveForDelete = 5;


        public void Execute()
        {
            AccountManager manager = AccountManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Display an Order");
            Console.WriteLine($"{Menu.stars}");

            // Validates Date and parses to DateTime
            do
            {
                // resets date
                Date.OrderDate = Convert.ToDateTime(DATE_TIME_ORIGIN);

                try
                {
                    Date.OrderDate = DateTime.Parse(ConsoleInput.ConsoleInput.GetStringFromUser(Date.DatePrompt));
                }
                catch
                {
                    Console.WriteLine("Date must be a legitimate date in the future.");
                }
            } while (Date.OrderDate.Date.ToString() == DATE_TIME_ORIGIN);

            // Sends response to the Data Layer to see if the file exists to be edited or deleted
            OrderLookupResponse displayListResponse = manager.DisplayOrder(Date.OrderDate);

            // Outputs what is returned from Data Layer
            ConsoleIO.DisplayOrderListDetails(displayListResponse.Order);

            int orderNumber = ConsoleInput.ConsoleInput.GetIntFromUser("Enter the order number of the order you would like to edit or delete: ", 1, MAX_INT);

            // Sends another response to edit a certain field or delete an order
            AddEditOrDeleteOrderResponse responseCheck = manager.EditOrDeleteOrderSelection(Date.OrderDate, orderNumber);

            if (responseCheck.Success)
            {
                Console.WriteLine($"1. {responseCheck.Order.CustomerName}");
                Console.WriteLine($"2. {responseCheck.Order.State}");
                Console.WriteLine($"3. {responseCheck.Order.ProductType}");
                Console.WriteLine($"4. {responseCheck.Order.Area}");
                Console.WriteLine("5. Delete Order");

                int editNumber = ConsoleInput.ConsoleInput.GetIntFromUser("Enter the item you would like to edit or hit 5 to delete an order: ", 1, _fiveForDelete);

                // Validates the user's responses and assigns them to the appropriate Order properties
                switch (editNumber)
                {
                    case 1:
                        do
                        {
                            responseCheck.Order.CustomerName = ConsoleInput.ConsoleInput.GetStringFromUser("Enter customer's name (a-z 0-9 , or . are excepted): ");

                        } while (!(Regex.IsMatch(responseCheck.Order.CustomerName, @"^[a-zA-Z0-9., ]+$")));
                        break;
                    case 2:
                        do
                        {
                            responseCheck.Order.State = ConsoleInput.ConsoleInput.GetStringFromUser("Enter the state abbreviation (PA, OH, MI, or IN): ").ToUpper();
                        } while (!(responseCheck.Order.State == Taxes.statePA || responseCheck.Order.State == Taxes.stateOH ||
                                   responseCheck.Order.State == Taxes.stateMI || responseCheck.Order.State == Taxes.stateIN));
                        break;
                    case 3:
                        do
                        {
                            responseCheck.Order.ProductType = ConsoleInput.ConsoleInput.GetStringFromUser("Enter a product ( Carpet, Laminate, Tile, or Wood ): ");
                        } while (!(responseCheck.Order.ProductType.ToUpper() == Products.typeCarpet ||
                                   responseCheck.Order.ProductType.ToUpper() == Products.typeLaminate ||
                                   responseCheck.Order.ProductType.ToUpper() == Products.typeTile ||
                                   responseCheck.Order.ProductType.ToUpper() == Products.typeWood));
                        break;
                    case 4:
                        responseCheck.Order.Area = ConsoleInput.ConsoleInput.GetDecimalFromUser("Enter your changes: ", Order.MIN_AREA, MAX_INT);
                        break;
                    case 5:
                        responseCheck.Order.CustomerName = $"{responseCheck.Order.CustomerName} DELETE";
                        break;
                    default:
                        return;
                }

                // Displays what you are editing or deleting
                ConsoleIO.DisplayOrderDetails(responseCheck.Order);

                // Prompts you to confirm
                string response;
                do
                {
                    response = ConsoleInput.ConsoleInput.GetStringFromUser("Would you like to continue with the changes above? (Yes or No) ").ToUpper();
                } while (response != "YES" && response != "NO");

                // If yes then send to the Data Layer to edit or delete
                if (response == "YES")
                {
                    AddEditOrDeleteOrderResponse responseEdit = manager.EditOrDeleteOrder(responseCheck.Order);
                    if (responseEdit.Success)
                    {
                        ConsoleIO.DisplayOrderDetails(responseEdit.Order);
                    }
                    else
                    {
                        Console.WriteLine("An error occurred: ");
                        Console.WriteLine(responseEdit.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("An error occurred: ");
                Console.WriteLine(responseCheck.Message);
            }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
