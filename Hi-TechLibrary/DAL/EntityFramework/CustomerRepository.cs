using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class CustomerRepository
    {
        private readonly HiTechDBEntities dbContext;

        public CustomerRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        public IEnumerable<Customers> GetAllCustomers()
        {
            return dbContext.Customers.ToList();
        }

        #region CRUD Operations
        public void AddCustomer(Customers customer)
        {
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();
        }
        public Customers GetCustomerById(int id)
        {
            return dbContext.Customers.Find(id);
        }
        public Customers GetCustomerByName(string name)
        {
            return dbContext.Customers.FirstOrDefault(c => c.Name == name);
        }
        #endregion
    }
}
