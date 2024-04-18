using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class OrderDetailRepository
    {
        private readonly HiTechDBEntities dbContext;

        public OrderDetailRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        public IEnumerable<OrderDetails> GetAllOrderDetails()
        {
            return dbContext.OrderDetails.ToList();
        }

        #region CRUD Operations
        public void AddOrderDetail(OrderDetails orderDetail)
        {
            dbContext.OrderDetails.Add(orderDetail);
            dbContext.SaveChanges();
        }
        public OrderDetails GetOrderDetailById(int id)
        {
            return dbContext.OrderDetails.Find(id);
        }
        public void UpdateOrderDetail(OrderDetails orderDetail)
        {
            var existingOrderDetail = dbContext.OrderDetails.Find(orderDetail.OrderDetailID);
            if (existingOrderDetail != null)
            {
                existingOrderDetail.OrderID = orderDetail.OrderID;
                existingOrderDetail.BookID = orderDetail.BookID;
                existingOrderDetail.Quantity = orderDetail.Quantity;
                existingOrderDetail.Price = orderDetail.Price;
                dbContext.SaveChanges();
            }
        }
        public void DeleteOrderDetail(int id)
        {
            var orderDetail = dbContext.OrderDetails.Find(id);
            if (orderDetail != null)
            {
                dbContext.OrderDetails.Remove(orderDetail);
                dbContext.SaveChanges();
            }
        }
        #endregion
    }
}
