using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models.Responses
{
    public class AddEditOrDeleteOrderResponse : Response
    {
        public Order Order { get; set; }
        public AddEditOrDeleteOrderResponse()
        {
        }
    }
}
