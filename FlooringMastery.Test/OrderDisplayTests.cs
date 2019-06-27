using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringMastery.BLL;
using FlooringMastery.Data;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using NUnit.Framework;

namespace FlooringMastery.Test
{
    [TestFixture]
    public class AAOrderDisplayTests
    {
        [TestCase("1/1/2020 12:00:00 AM", true)] // account exists and can be found
        [TestCase("1/3/2020 12:00:00 AM", true)] // account exists and can be found
        [TestCase("6/9/2040 12:00:00 AM", false)] // account does not exist
        [TestCase("31/31/2020 12:00:00 AM", false)] // Not a valid date
        [TestCase("timmy", false)] // not a valid date, random string
        [TestCase("", false)] // empty string


        public void DisplayOrderTest(string date, bool expectedResult)
        {
            AccountManager manager = AccountManagerFactory.Create();

            OrderLookupResponse response = manager.DisplayOrder(date);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
