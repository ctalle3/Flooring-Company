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
    public class AddOrderTests
    {
        [TestCase("1/1/2020 12:00:00 AM", "Bill", "PA", "Carpet", 1000, true)] // account exists and can be found
        [TestCase("1/1/2020 12:00:00 AM", "Jim", "MI", "WOOD", 1000000, true)] // account exists and can be found
        [TestCase("1/1/2020 12:00:00 AM", "Jim", "mi", "Wood", 1000000, true)] // account exists and can be found
        [TestCase("6/5/2020 12:00:00 AM", "Bill", "OH", "Tile", 800, true)] // account added
        [TestCase("31/31/2020 12:00:00 AM", "Bill", "IN", "Wood", 150, false)] // Not a valid date
        [TestCase("timmy", "Bill", "MI", "Wood", 125, false)] // not a valid date, random string
        [TestCase("", "Bill", "MI", "Wood", 150, false)] // not a valid date, empty string
        [TestCase("1/1/2020", "Bill", "PA", "Tile", 10, false)] //Area less than 100
        [TestCase("1/1/2020", "Bill", "PA", "Dirt", 1000, false)] //ProductType not proper type
        [TestCase("1/1/2020", "Bill", "KY", "Tile", 1000, false)] //State not proper state
        [TestCase("1/1/2020", "Bi*ll", "MI", "Tile", 1000, false)] //Name not in proper format

        public void AddOrderTest(string date, string name, string state, 
                                 string productType, decimal area, bool expectedResult)
        {
            Order order = new Order
            {
                CustomerName = name,
                State = state,
                ProductType = productType,
                Area = area
            };

            AccountManager manager = AccountManagerFactory.Create();

            AddEditOrDeleteOrderResponse response = manager.AddOrder(date, order);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
