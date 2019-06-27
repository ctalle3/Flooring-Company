using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public class Taxes
    {
        // Removes Magic Numbers
        public static string stateOH = "OH";
        public static string statePA = "PA";
        public static string stateIN = "IN";
        public static string stateMI = "MI";

        public string StateAbbreviation { get; set; }
        public string StateName { get; set; }   
        public decimal TaxRate { get; set; }
    }
}
