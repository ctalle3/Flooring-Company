using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public class Date
    {
        public static string DatePrompt = "Enter the date of the order to display ( MM/DD/YYYY ): ";
        public const string DATE_TIME_ORIGIN = "1/1/0001 12:00:00 AM";
        public static DateTime OrderDate { get; set; } 
    }
}
