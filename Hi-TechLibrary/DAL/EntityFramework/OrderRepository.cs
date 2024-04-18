using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class OrderRepository
    {
        private readonly HiTechDBEntities dbContext;

        public OrderRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        public IEnumerable<Orders> GetAllOrders()
        {
            return dbContext.Orders.ToList();
        }

        #region CRUD Operations
        public void AddOrder(Orders order)
        {
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
        }
        public Orders GetOrderById(int id)
        {
            return dbContext.Orders.Find(id);
        }
        public void UpdateOrder(Orders order)
        {
            var existingOrder = dbContext.Orders.Find(order.OrderID);
            if (existingOrder != null)
            {
                existingOrder.CustomerID = order.CustomerID;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.TotalAmount = order.TotalAmount;
                existingOrder.StatusID = order.StatusID;
                existingOrder.DateModified = DateTime.Now;
                dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Order not found with ID: " + order.OrderID);
            }
        }

        #endregion

        public void CancelOrder(int id)
        {
            var order = dbContext.Orders.Find(id);
            if (order != null)
            {
                order.StatusID = 3;
                dbContext.SaveChanges();
            }
        }
    }
}
