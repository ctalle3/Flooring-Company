using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> DisplayOrder(DateTime orderDate);
        Order EditOrDeleteOrderSelection(DateTime orderDate, int orderNumber);
        Order EditOrDeleteOrder(Order order);
        Order AddOrder(DateTime orderDate, Order order);
    }
}
