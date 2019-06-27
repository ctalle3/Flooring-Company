using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public class Products
    {
        // Removes Magic Numbers
        public static string typeWood = "WOOD";
        public static string typeTile = "TILE";
        public static string typeCarpet = "CARPET";
        public static string typeLaminate = "LAMINATE";

        public string ProductType { get; set; }
        public decimal CostPerSquareFoot { get; set; }  
        public decimal LaborCostPerSquareFoot { get; set; }
    }
}
