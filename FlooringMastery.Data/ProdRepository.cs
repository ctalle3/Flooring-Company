using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlooringMastery.Data
{
    public class ProdRepository : IOrderRepository
    {
        // removes magic numbers
        private string _header = "OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total";
        private string _path;
        private string _taxPath = "Taxes.txt";
        private string _productsPath = "Products.txt";

        private const int ZERO = 0;
        private const int ONE = 1;
        private const int TWO = 2;
        private const int THREE = 3;
        private const int FOUR = 4;
        private const int FIVE = 5;
        private const int SIX = 6;
        private const int SEVEN = 7;
        private const int EIGHT = 8;
        private const int NINE = 9;
        private const int TEN = 10;
        private const int ELEVEN = 11;
        private const int GET_PERCENT = 100;

        // Helper function that splits the date
        private string[] DateSplit(DateTime orderDate)
        {
            // splits time into 3 components and then makes an array
            string month = orderDate.Date.Month.ToString();
            string day = orderDate.Date.Day.ToString();
            string year = orderDate.Date.Year.ToString();

            string[] splitDate = new string[THREE] {month,day,year};

            return splitDate;
        }

        // generates Tax Repository
        private List<Taxes> TaxesRepositoryList()
        {
            List<Taxes> taxes = new List<Taxes>();

            if (File.Exists(_taxPath))
            {
                using(StreamReader sr = new StreamReader(_taxPath))
                {
                    sr.ReadLine();
                    string line;

                    while((line = sr.ReadLine()) != null)
                    {
                        Taxes newTaxes = new Taxes();

                        string[] columns = line.Split(',');

                        newTaxes.StateAbbreviation = columns[0];
                        newTaxes.StateName = columns[1];
                        newTaxes.TaxRate = Convert.ToDecimal(columns[TWO]);

                        taxes.Add(newTaxes);
                    }
                }
                return taxes;
            }
            else
            {
                return null;
            }
        }

        // generates products repository
        private List<Products> ProductsRepositoryList()
        {
            List<Products> products = new List<Products>();

            if (File.Exists(_productsPath))
            {
                using (StreamReader sr = new StreamReader(_productsPath))
                {
                    sr.ReadLine();
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Products newProduct = new Products();

                        string[] columns = line.Split(',');

                        newProduct.ProductType = columns[0];
                        newProduct.CostPerSquareFoot = Convert.ToDecimal(columns[1]);
                        newProduct.LaborCostPerSquareFoot = Convert.ToDecimal(columns[TWO]);

                        products.Add(newProduct);
                    }
                }
                return products;
            }
            else
            {
                return null;
            }
        }

        // generates order repository
        private List<Order> OrderRepositoryList()
        {
            List<Order> orders = new List<Order>();

            GetFilePath(Date.OrderDate);

            if (File.Exists(_path))
            {
                using (StreamReader sr = new StreamReader(_path))
                {
                    sr.ReadLine();
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Order newOrder = new Order();

                        string[] columns;

                        if (line.Contains('"'))
                        {
                            string[] columnQuoteSplit = line.Split('"');
                            columns = (columnQuoteSplit[0] + columnQuoteSplit[1].Replace(",", "") + columnQuoteSplit[2]).ToString().Split(',');
                            columns[1] = columnQuoteSplit[1];
                        }
                        else
                        {
                            columns = line.Split(',');
                        }

                        newOrder.OrderNumber = int.Parse(columns[ZERO]);
                        newOrder.CustomerName = columns[ONE];
                        newOrder.State = columns[TWO];
                        newOrder.TaxRate = Convert.ToDecimal(columns[THREE]);
                        newOrder.ProductType = columns[FOUR];
                        newOrder.Area = Convert.ToDecimal(columns[FIVE]);
                        newOrder.CostPerSquareFoot = Convert.ToDecimal(columns[SIX]);
                        newOrder.LaborCostPerSquareFoot = Convert.ToDecimal(columns[SEVEN]);
                        newOrder.MaterialCost = Convert.ToDecimal(columns[EIGHT]);
                        newOrder.LaborCost = Convert.ToDecimal(columns[NINE]);
                        newOrder.Tax = Convert.ToDecimal(columns[TEN]);
                        newOrder.Total = Convert.ToDecimal(columns[ELEVEN]);

                        orders.Add(newOrder);
                    }
                }

                return orders;
            }
            else
            {
                return null;
            }
        }

        // Helper function that calls splitDate and generates the path needed to read/write files
        private string GetFilePath(DateTime orderDate)
        {
            string[] splitDate = DateSplit(Date.OrderDate);
            return _path = $"Orders_{splitDate[ZERO]}{splitDate[ONE]}{splitDate[TWO]}.txt";
        }

        // Helper function that Calculates the MaterialCost, LaborCost, Tax, and Total
        public Order Calculations(Order order)
        {
            List<Products> products = ProductsRepositoryList();
            List<Taxes> taxes = TaxesRepositoryList();

            foreach(Products product in products.Where(p => p.ProductType.ToUpper() == order.ProductType.ToUpper()))
            {
                foreach(Taxes tax in taxes.Where(t => t.StateAbbreviation.ToUpper() == order.State.ToUpper()))
                {
                    order.MaterialCost = Math.Round((order.Area * product.CostPerSquareFoot), TWO);
                    order.LaborCost = Math.Round((order.Area * product.LaborCostPerSquareFoot), TWO);
                    order.Tax = Math.Round((order.MaterialCost + order.LaborCost) * (tax.TaxRate / GET_PERCENT), TWO);
                    order.Total = Math.Round((order.MaterialCost + order.LaborCost + order.Tax), TWO);
                }
            }
            return order;
        }

        // Helper function that Creates a CSV file
        public string CreateCSV(Order order)
       {
            return string.Format($"{order.OrderNumber},{order.CustomerName},{order.State},{order.TaxRate},{order.ProductType},{order.Area}," +
                                 $"{order.CostPerSquareFoot},{order.LaborCostPerSquareFoot},{order.MaterialCost},{order.LaborCost},{order.Tax},{order.Total}");     
       }

        // Displays an existing order
        public List<Order> DisplayOrder(DateTime orderDate)
        {
            List<Order> orders = OrderRepositoryList();

            if(orders != null)
            {
                return orders.OrderBy(o => o.OrderNumber).ToList();
            }
            return null;
        }

        // Helper funciton that writes to a file
        public void WriteToNewFileOrOrder(Order order, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(_header);
                sw.WriteLine(CreateCSV(order));
            }
        }

        // Adds an order to a file or creates a new file if no file exists
        public Order AddOrder(DateTime orderDate, Order order)
        {
            List<Order> orders = OrderRepositoryList();
            List<Taxes> taxes = TaxesRepositoryList();
            List<Products> products = ProductsRepositoryList();
            GetFilePath(orderDate);
            Order addedOrder = new Order();
            DateTime currentDate = DateTime.Parse(DateTime.Now.Date.ToShortDateString());

            // forces futer dating
            if (currentDate < Date.OrderDate)
            {
                // adds paranthesis around CustomerName in file (eventually)
                addedOrder.CustomerName = $"\"{order.CustomerName}\"";

                // assigns tax and state abbreviation from tax repository
                foreach (Taxes tax in taxes.Where(t => t.StateAbbreviation.ToUpper() == order.State.ToUpper()))
                {
                    addedOrder.State = tax.StateAbbreviation;
                    addedOrder.TaxRate = tax.TaxRate;
                }

                addedOrder.Area = order.Area;

                // assigns Product properties
                foreach(Products product in products.Where(p => p.ProductType.ToUpper() == order.ProductType.ToUpper()))
                {
                    addedOrder.ProductType = product.ProductType;
                    addedOrder.CostPerSquareFoot = product.CostPerSquareFoot;
                    addedOrder.LaborCostPerSquareFoot = product.LaborCostPerSquareFoot;
                }

                // calculates remaining properties
                Calculations(addedOrder);

                //Check to see if file exists
                if (File.Exists(_path))
                {
                    //Finds max orderNumber in List of orders
                    addedOrder.OrderNumber = orders.Max(o => o.OrderNumber) + 1;

                    //Write to existing file
                    using(StreamWriter sw = File.AppendText(_path))
                    {
                        sw.WriteLine(CreateCSV(addedOrder));
                    }
                }
                else
                {   
                    //File doesn't exist. So create it
                    addedOrder.OrderNumber = 1;

                    using (File.Create(_path)){} 

                    //Write to new file
                    WriteToNewFileOrOrder(addedOrder, _path);
                } 
            }
            else
            {
                Console.WriteLine("Orders must be future dated.");
                return addedOrder = null;
            }
            //Return order that has been added
            return addedOrder;
        }

        public Order EditOrDeleteOrderSelection(DateTime orderDate, int orderNumber)
        {
            List<Order> orders = OrderRepositoryList();

            Order chosenOrder = null;

            GetFilePath(orderDate);

            // makes sure file exists
            if (File.Exists(_path))
            {
                // loops over orders to find order that will be edited or deleted
                foreach (var order in orders)
                {
                    if (order.OrderNumber == orderNumber)
                    {
                        chosenOrder = order;
                        break;
                    }
                }
            }

            return chosenOrder;
        }

        public Order EditOrDeleteOrder(Order order)
        {
            List<Order> orders = OrderRepositoryList();
            List<Taxes> taxes = TaxesRepositoryList();
            List<Products> products = ProductsRepositoryList();

            Order editedOrder = null;

            // checks for delete tag
            if (!order.CustomerName.Contains("DELETE"))
            {
                // loops to check for changes in product type
                foreach (Products product in products.Where(p => p.ProductType.ToUpper() == order.ProductType.ToUpper()))
                {
                    // loops to check for state abbreviation 
                    foreach (Taxes tax in taxes.Where(t => t.StateAbbreviation == order.State.ToUpper()))
                    {
                        // writes to file
                        using (StreamWriter sw = new StreamWriter(_path))
                        {
                            sw.WriteLine(_header);

                            // creates a CSV file for the order edited
                            foreach (Order ord in orders.Where(o => o.OrderNumber == order.OrderNumber))
                            {
                                ord.CustomerName = $"\"{ord.CustomerName}\"";
                                sw.WriteLine(CreateCSV(order));
                            }

                            // creates a CSV file for all the other orders
                            foreach (Order o in orders.Where(o => o.OrderNumber != order.OrderNumber))
                            {
                                o.CustomerName = $"\"{o.CustomerName}\"";
                                sw.WriteLine(CreateCSV(o));
                            }
                        }
                        editedOrder = Calculations(order);
                    }
                }
            }
            // contains delete tag
            else
            {
                // write to file
                using (StreamWriter sw = new StreamWriter(_path))
                {
                    sw.WriteLine(_header);

                    // writes all the orders to file besides the one to be deleted
                    foreach (Order o in orders.Where(o => o.OrderNumber != order.OrderNumber))
                    {
                        o.CustomerName = $"\"{o.CustomerName}\"";
                        sw.WriteLine(CreateCSV(o));
                    }
                }
                // changes tag to DELETED from DELETE for output purposes
                order.CustomerName = $"{order.CustomerName}D";
                editedOrder = order;
            }
            return editedOrder;
        }
    }
}
