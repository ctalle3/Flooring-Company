using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using NUnit.Framework;

namespace FlooringMastery.Test
{
    [TestFixture]
    public class EditOrDeleteOrderTests
    {
        [TestCase("1/1/2020 12:00:00 AM", "1", true)] // account exists and can be found
        [TestCase("1/3/2020 12:00:00 AM", "2", true)] // account exists and can be found
        [TestCase("31/31/2020 12:00:00 AM", "1", false)] // Not a valid date
        [TestCase("timmy", "1", false)] // not a valid date, random string
        [TestCase("", "1", false)] // empty string
        [TestCase("1/1/2020 12:00:00 AM", "500", false)] // account exists and can be found
        [TestCase("1/3/2020 12:00:00 AM", "5", false)] // account exists and can be found
        [TestCase("1/1/2020 12:00:00 AM", "0", false)] // account exists and can be found
        [TestCase("1/3/2020 12:00:00 AM", "0", false)] // account exists and can be found

        public void EditOrDeleteOrderSelectionTest(string date, string orderNumber, bool expectedResult)
        {
            AccountManager manager = AccountManagerFactory.Create();

            AddEditOrDeleteOrderResponse response = manager.EditOrDeleteOrderSelection(date, orderNumber);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("1", "Bill", "PA", "Tile", 1000, true)] //Edits the name, state, productType, and area correctly
        [TestCase("1", "Bill", "in", "Tile", 1000, true)] //Edits the name, state, productType, and area
        [TestCase("1", "Bill", "in", "WOOD", 1000, true)] //Edits the name, state, productType, and area
        [TestCase("1", "Bill", "PA", "Tile", 10, false)] //Area less than 100
        [TestCase("1", "Bill", "PA", "Dirt", 1000, false)] //ProductType not proper type
        [TestCase("1", "Bill", "KY", "Tile", 1000, false)] //State not proper state
        [TestCase("1", "Bi*ll", "PA", "Tile", 1000, false)] //Name not in proper format

        public void EditOrDeleteOrderTest(int orderNumber, string name, 
                                          string state, string productType, 
                                          decimal area, bool expectedResult)
        {
            Order orderTest = new Order
            {
                OrderNumber = 1,
                CustomerName = name,
                State = state,
                ProductType = productType,
                Area = area
            };

            AccountManager manager = AccountManagerFactory.Create();

            AddEditOrDeleteOrderResponse response = manager.EditOrDeleteOrder(orderTest);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
