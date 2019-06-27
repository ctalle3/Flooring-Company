using FlooringMastery.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery
{
    public class Menu
    {
        public const string stars = "********************";

        public static void Start()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"{stars}");
                Console.WriteLine("Flooring Program\n");
                Console.WriteLine("1. Display Orders");
                Console.WriteLine("2. Add an Order");
                Console.WriteLine("3. Edit or Delete an Order");
                Console.WriteLine("4. Quit\n");

                string userInput = ConsoleInput.ConsoleInput.GetStringFromUser($"Enter a selection.\n{stars}\n");

                // Calls appropriate workflow for choice given above
                switch (userInput)
                {
                    case "1":
                        DisplayOrderWorkflow displayWorkflow = new DisplayOrderWorkflow();
                        displayWorkflow.Execute();
                        break;
                    case "2":
                        AddOrderWorkflow addWorkflow = new AddOrderWorkflow();
                        addWorkflow.Execute();
                        break;
                    case "3":
                        EditOrDeleteOrderWorkflow editOrDeleteWorkflow = new EditOrDeleteOrderWorkflow();
                        editOrDeleteWorkflow.Execute();
                        break;
                    case "4":
                        return;
                }
            }
        }
    }
}
