using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class OrderController
    {
        private readonly OrderRepository orderRepository = new OrderRepository();

        public IEnumerable<Orders> GetAllOrders()
        {
            return orderRepository.GetAllOrders();
        }

        public void AddOrder(Orders order)
        {
            orderRepository.AddOrder(order);
        }

        public Orders GetOrderById(int id)
        {
            return orderRepository.GetOrderById(id);
        }

        public void UpdateOrder(Orders order)
        {
            orderRepository.UpdateOrder(order);
        }

        public void CancelOrder(int id)
        {
            orderRepository.CancelOrder(id);
        }
    }
}
