using FlooringMastery.Data;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public static class AccountManagerFactory
    {
        public static AccountManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch (mode)
            {
                case "Test":
                    return new AccountManager(new TestRepository());
                case "Prod":
                    return new AccountManager(new ProdRepository());
                default:
                    throw new Exception("Mode value in app config is not valid.");
            }
        }
    }
}
