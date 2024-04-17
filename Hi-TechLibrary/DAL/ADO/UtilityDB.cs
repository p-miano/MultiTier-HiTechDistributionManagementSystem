using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Hi_TechLibrary.DAL.ADO
{
    public static class UtilityDB
    {
        public static SqlConnection GetDBConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString; // Pass the name of the connection string to the ConnectionString property. The connection string is stored in the App.config file.
            conn.Open();
            return conn;
        }
    }
}
