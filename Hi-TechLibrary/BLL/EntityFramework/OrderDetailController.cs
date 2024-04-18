using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class OrderDetailController
    {
        private readonly OrderDetailRepository orderDetailRepository = new OrderDetailRepository();

        public IEnumerable<OrderDetails> GetAllOrderDetails()
        {
            return orderDetailRepository.GetAllOrderDetails();
        }

        public void AddOrderDetail(OrderDetails orderDetail)
        {
            orderDetailRepository.AddOrderDetail(orderDetail);
        }

        public OrderDetails GetOrderDetailById(int id)
        {
            return orderDetailRepository.GetOrderDetailById(id);
        }

        public void UpdateOrderDetail(OrderDetails orderDetail)
        {
            orderDetailRepository.UpdateOrderDetail(orderDetail);
        }

        public void DeleteOrderDetail(int id)
        {
            orderDetailRepository.DeleteOrderDetail(id);
        }
    }
}
