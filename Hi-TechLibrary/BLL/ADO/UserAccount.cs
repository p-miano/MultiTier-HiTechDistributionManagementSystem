using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.ADO;

namespace Hi_TechLibrary.BLL.ADO
{
    public class UserAccount
    {
        // Properties
        public int UserID { get; set; } 
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int StatusID { get; set; }
        public bool MustChangePassword { get; set; }

        private static readonly Random Random = new Random(); // Random number generator for password generation

        // Default constructor
        public UserAccount()
        {
            Username = string.Empty;
            Password = string.Empty;
            UserRole = UserRole.Default;
            StatusID = 1; // Default status is Active
            MustChangePassword = true; // New users must change their password
        }

        // Parameterized constructor for new user creation
        public UserAccount(int employeeID, string username, string password, UserRole userRole, int statusID)
        {
            EmployeeID = employeeID;
            Username = username;
            Password = password; 
            UserRole = userRole;
            StatusID = statusID;
            MustChangePassword = true;
        }

        // Overloaded constructor for retrieving an existing user from the database
        public UserAccount(int userID, int employeeID, string username, string password, UserRole userRole, DateTime dateCreated, DateTime? dateModified, int statusID, bool mustChangePassword)
        {
            UserID = userID;
            EmployeeID = employeeID;
            Username = username;
            Password = password;
            UserRole = userRole;
            DateCreated = dateCreated;
            DateModified = dateModified;
            StatusID = statusID;
            MustChangePassword = mustChangePassword;
        }

        // Method to save a user to the database
        public void SaveUserAccount(UserAccount userAccount)
        {
            UserAccountDB.SaveRecord(userAccount);
        }

        // Method to get all users from the database
        public List<UserAccount> GetAllUserAccounts()
        {
            return UserAccountDB.GetAllRecords();
        }
        // Method to search for a user account by UserAccountID
        public UserAccount SearchUserAccountByEmployeeId(int id)
        {
            return UserAccountDB.SearchByEmployeeId(id);
        }
        // Method to search for a user account by UserAccountID
        public UserAccount SearchUserAccountByUserAccountId(int id)
        {
            return UserAccountDB.SearchByUserAccountId(id);
        }

        // Method to search for a user by Username
        public UserAccount SearchUserAccountByUsername(string username)
        {
            return UserAccountDB.SearchByUsername(username);
        }

        // Method to search for a user by UserRole
        public List<UserAccount> SearchUserAccountByUserRole(UserRole userRole)
        {
            return UserAccountDB.SearchByUserRole(userRole);
        }

        // Method to update a user in the database
        public void UpdateUserAccount(UserAccount user)
        {
            UserAccountDB.UpdateRecord(user);
        }

        // Method to delete a user from the database
        public void DeleteUserAccount(int userID)
        {
            UserAccountDB.DeleteRecord(userID);
        }

        // Method to generate a random password
        public static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
