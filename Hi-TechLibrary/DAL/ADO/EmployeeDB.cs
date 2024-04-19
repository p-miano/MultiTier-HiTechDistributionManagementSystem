using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Hi_TechLibrary.BLL.ADO;

namespace Hi_TechLibrary.DAL.ADO
{
    public static class EmployeeDB
    {
        public static int SaveRecord(Employee employee)
        {
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                string insertQuery = @"
                INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber, PositionID, StatusID) 
                VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @PositionID, @StatusID);
                SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Email", employee.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                cmd.Parameters.AddWithValue("@PositionID", employee.PositionID);
                cmd.Parameters.AddWithValue("@StatusID", employee.StatusID);
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
        public static List<Employee> GetAllRecords()
        {
            List<Employee> listEmployees = new List<Employee>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Employee employee;
                while (reader.Read())
                {
                    employee = new Employee(Convert.ToInt32(reader["EmployeeID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Email"].ToString(), reader["PhoneNumber"].ToString(), Convert.ToInt32(reader["PositionID"]), Convert.ToInt32(reader["StatusID"]));
                    listEmployees.Add(employee);
                }
            }
            return listEmployees;
        }

        public static Employee SearchById(int employeeID)
        {
            Employee employee = new Employee();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EmployeeID", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee(Convert.ToInt32(reader["EmployeeID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Email"].ToString(), reader["PhoneNumber"].ToString(), Convert.ToInt32(reader["PositionID"]), Convert.ToInt32(reader["StatusID"]));
                }
                else
                {
                    employee = null;
                }
            }
            return employee;
        }

        public static Employee SearchByEmail(string email)
        {
            Employee employee = new Employee();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE Email = @SearchStr", conn);
                cmd.Parameters.AddWithValue("@SearchStr", email);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee(Convert.ToInt32(reader["EmployeeID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Email"].ToString(), reader["PhoneNumber"].ToString(), Convert.ToInt32(reader["PositionID"]), Convert.ToInt32(reader["StatusID"]));
                }
                else
                {
                    employee = null;
                }
            }
            return employee;
        }

        public static List<Employee> SearchByPosition(int positionID)
        {
            List<Employee> listEmployees = new List<Employee>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE PositionID = @PositionID", conn);
                cmd.Parameters.AddWithValue("@PositionID", positionID);
                SqlDataReader reader = cmd.ExecuteReader();
                Employee employee;
                while (reader.Read())
                {
                    employee = new Employee(Convert.ToInt32(reader["EmployeeID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Email"].ToString(), reader["PhoneNumber"].ToString(), Convert.ToInt32(reader["PositionID"]), Convert.ToInt32(reader["StatusID"]));
                    listEmployees.Add(employee);
                }
            }
            return listEmployees;
        }

        public static List<Employee> SearchByNameOrPhone(string searchStr)
        {
            List<Employee> listEmployees = new List<Employee>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE FirstName LIKE @SearchStr OR LastName LIKE @SearchStr OR PhoneNumber LIKE @SearchStr", conn);
                cmd.Parameters.AddWithValue("@SearchStr", "%" + searchStr + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                Employee employee;
                while (reader.Read())
                {
                    employee = new Employee(Convert.ToInt32(reader["EmployeeID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Email"].ToString(), reader["PhoneNumber"].ToString(), Convert.ToInt32(reader["PositionID"]), Convert.ToInt32(reader["StatusID"]));
                    listEmployees.Add(employee);
                }
            }
            return listEmployees;
        }

        public static List<Employee> SearchByFirstAndLastName(string firstName, string lastName)
        {
            List<Employee> listEmployees = new List<Employee>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName", conn);
                cmd.Parameters.AddWithValue("@FirstName", "%" + firstName + "%");
                cmd.Parameters.AddWithValue("@LastName", "%" + lastName + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                Employee employee;
                while (reader.Read())
                {
                    employee = new Employee(Convert.ToInt32(reader["EmployeeID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Email"].ToString(), reader["PhoneNumber"].ToString(), Convert.ToInt32(reader["PositionID"]), Convert.ToInt32(reader["StatusID"]));
                    listEmployees.Add(employee);
                }
            }
            return listEmployees;
        }

        public static void UpdateRecord(Employee employee)
        {
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber, PositionID = @PositionID, StatusID = @StatusID WHERE EmployeeID = @EmployeeID", conn);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Email", employee.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                cmd.Parameters.AddWithValue("@PositionID", employee.PositionID);
                cmd.Parameters.AddWithValue("@StatusID", employee.StatusID);
                cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRecord(int employeeID)
        {
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
