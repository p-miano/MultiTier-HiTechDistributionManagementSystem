using Hi_TechLibrary.DAL.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hi_TechLibrary.BLL.ADO
{
    public class Employee
    {
        // Properties
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int PositionID { get; set; }
        public int StatusID { get; set; }

        // Default constructor
        public Employee()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            PositionID = 0;
            StatusID = 1; // Default status is Active
        }

        // Parameterized constructor for new employee creation. Database will generate EmployeeID.
        public Employee(string firstName, string lastName, string email, string phoneNumber, int positionID)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            PositionID = positionID;
            StatusID = 1;
        }

        // Overloaded constructor for retrieving an existing employee from the database.
        public Employee(int employeeID, string firstName, string lastName, string email, string phoneNumber, int positionID, int statusID)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            PositionID = positionID;
            StatusID = statusID;
        }

        // Method to save an employee to the database. Returns the EmployeeID of the new record.
        public int SaveEmployee(Employee employee)
        {
            return EmployeeDB.SaveRecord(employee);
        }

        // Method to get all employees from the database
        public List<Employee> GetAllEmployees()
        {
            return EmployeeDB.GetAllRecords();
        }

        // Method to search for an employee by EmployeeID
        public Employee SearchEmployeeById(int employeeID)
        {
            return EmployeeDB.SearchById(employeeID);
        }

        // Method to search for an employee by email
        public Employee SearchEmployeeByEmail(string email)
        {
            return EmployeeDB.SearchByEmail(email);
        }

        // Method to search for employees by position
        public List<Employee> SearchEmployeesByPosition(int positionID)
        {
            return EmployeeDB.SearchByPosition(positionID);
        }

        // Method to search for an employee by first name, last name or phone number
        public List<Employee> SearchEmployees(string searchStr)
        {
            return EmployeeDB.SearchByNameOrPhone(searchStr);
        }

        // Overloaded method to search for an employee by first name and last name together
        public List<Employee> SearchEmployees(string firstName, string lastName)
        {
            return EmployeeDB.SearchByFirstAndLastName(firstName, lastName);
        }

        // Method to update an employee in the database
        public void UpdateEmployee(Employee employee)
        {
            EmployeeDB.UpdateRecord(employee);
        }

        // Method to delete an employee from the database
        public void DeleteEmployee(int employeeID)
        {
            EmployeeDB.DeleteRecord(employeeID);
        }
    }
}
