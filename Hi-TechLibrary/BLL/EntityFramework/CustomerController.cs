using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class CustomerController
    {
        private readonly CustomerRepository customerRepository;

        public CustomerController()
        {
            customerRepository = new CustomerRepository();
        }

        public IEnumerable<Customers> GetAllCustomers() => customerRepository.GetAllCustomers();
        public Customers GetCustomerById(int id) => customerRepository.GetCustomerById(id);
        public Customers GetCustomerByName(string name) => customerRepository.GetCustomerByName(name);
        public bool Exists(string customerName)
        {
            return customerRepository.GetAllCustomers().Any(c => c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
