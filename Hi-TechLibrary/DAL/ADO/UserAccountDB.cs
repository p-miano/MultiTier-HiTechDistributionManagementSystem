using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.ADO;

namespace Hi_TechLibrary.DAL.ADO
{
    public static class UserAccountDB
    {
        public static void SaveRecord(UserAccount userAccount)
        {
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO UserAccounts (EmployeeID, Username, Password, UserRole, StatusID, MustChangePassword) " +
                                                          "VALUES (@EmployeeID, @Username, @Password, @UserRole, @StatusID, @MustChangePassword)", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", userAccount.EmployeeID);
                cmd.Parameters.AddWithValue("@Username", userAccount.Username);
                cmd.Parameters.AddWithValue("@Password", userAccount.Password);
                cmd.Parameters.AddWithValue("@UserRole", userAccount.UserRole.ToString());
                cmd.Parameters.AddWithValue("@StatusID", userAccount.StatusID);
                cmd.Parameters.AddWithValue("@MustChangePassword", userAccount.MustChangePassword);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<UserAccount> GetAllRecords()
        {
            List<UserAccount> listUsers = new List<UserAccount>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserAccounts", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                UserAccount user;
                while (reader.Read())
                {
                    user = new UserAccount();
                    user.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    // Convert the UserRole from string to UserRole enum
                    string roleString = reader["UserRole"].ToString();
                    if (Enum.TryParse<UserRole>(roleString, out UserRole role))
                    {
                        user.UserRole = role;
                    }
                    else
                    {
                        user.UserRole = UserRole.Default;
                    }
                    user.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                    user.DateModified = reader["DateModified"] as DateTime?;
                    user.StatusID = Convert.ToInt32(reader["StatusID"]);
                    user.MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]);
                    listUsers.Add(user);
                }
            }
            return listUsers;
        }

        public static UserAccount SearchByEmployeeId(int id)
        {
            UserAccount user = new UserAccount();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserAccounts WHERE EmployeeID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    // Convert the UserRole from string to UserRole enum
                    string roleString = reader["UserRole"].ToString();
                    if (Enum.TryParse<UserRole>(roleString, out UserRole role))
                    {
                        user.UserRole = role;
                    }
                    else
                    {
                        user.UserRole = UserRole.Default;
                    }
                    user.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                    user.DateModified = reader["DateModified"] as DateTime?;
                    user.StatusID = Convert.ToInt32(reader["StatusID"]);
                    user.MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]);
                }
            }
            return user;
        }

        public static UserAccount SearchByUserAccountId(int id)
        {
            UserAccount user = new UserAccount();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserAccounts WHERE UserID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    // Convert the UserRole from string to UserRole enum
                    string roleString = reader["UserRole"].ToString();
                    if (Enum.TryParse<UserRole>(roleString, out UserRole role))
                    {
                        user.UserRole = role;
                    }
                    else
                    {
                        user.UserRole = UserRole.Default;
                    }
                    user.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                    user.DateModified = reader["DateModified"] as DateTime?;
                    user.StatusID = Convert.ToInt32(reader["StatusID"]);
                    user.MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]);
                }
            }
            return user;
        }

        public static UserAccount SearchByUsername(string username)
        {
            UserAccount user = new UserAccount();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserAccounts WHERE Username = @Username", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    // Convert the UserRole from string to UserRole enum
                    string roleString = reader["UserRole"].ToString();
                    if (Enum.TryParse<UserRole>(roleString, out UserRole role))
                    {
                        user.UserRole = role;
                    }
                    else
                    {
                        user.UserRole = UserRole.Default;
                    }
                    user.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                    user.DateModified = reader["DateModified"] as DateTime?;
                    user.StatusID = Convert.ToInt32(reader["StatusID"]);
                    user.MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]);
                }
            }
            return user;
        }

        public static List<UserAccount> SearchByUserRole(UserRole userRole)
        {
            List<UserAccount> listUsers = new List<UserAccount>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserAccounts WHERE UserRole = @UserRole", conn);
                // Convert the Enum to a string for the SQL query
                cmd.Parameters.AddWithValue("@UserRole", userRole.ToString());
                SqlDataReader reader = cmd.ExecuteReader();
                UserAccount user;
                while (reader.Read())
                {
                    user = new UserAccount();
                    user.EmployeeID = Convert.ToInt32(reader["EmployeeID"]);
                    user.UserID = Convert.ToInt32(reader["UserID"]);
                    user.Username = reader["Username"].ToString();
                    user.Password = reader["Password"].ToString();
                    // Since we're fetching by UserRole, we already know the role, no need to parse
                    user.UserRole = userRole;
                    user.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
                    user.DateModified = reader["DateModified"] as DateTime?;
                    user.StatusID = Convert.ToInt32(reader["StatusID"]);
                    user.MustChangePassword = Convert.ToBoolean(reader["MustChangePassword"]);
                    listUsers.Add(user);
                }
            }
            return listUsers;
        }

        public static void UpdateRecord(UserAccount userAccount)
        {
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("UPDATE UserAccounts SET Username = @Username, Password = @Password, UserRole = @UserRole, DateModified = @DateModified, StatusID = @StatusID, MustChangePassword = @MustChangePassword " +
                                       "WHERE UserID = @UserID", conn);
                cmd.Parameters.AddWithValue("@Username", userAccount.Username);
                cmd.Parameters.AddWithValue("@Password", userAccount.Password);
                cmd.Parameters.AddWithValue("@UserRole", userAccount.UserRole.ToString());
                cmd.Parameters.AddWithValue("@DateModified", userAccount.DateModified.HasValue ? (object)userAccount.DateModified.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusID", userAccount.StatusID);
                cmd.Parameters.AddWithValue("@MustChangePassword", userAccount.MustChangePassword);
                cmd.Parameters.AddWithValue("@UserID", userAccount.UserID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRecord(int userID)
        {
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM UserAccounts WHERE UserID = @UserID", conn);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
