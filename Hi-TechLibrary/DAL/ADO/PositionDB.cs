using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.ADO;

namespace Hi_TechLibrary.DAL.ADO
{
    public static class PositionDB
    {
        public static List<Position> GetAllRecords()
        {
            List<Position> listPositions = new List<Position>();
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Positions", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Position position;
                while (reader.Read())
                {
                    position = new Position(Convert.ToInt32(reader["PositionID"]), reader["PositionTitle"].ToString(), reader["PositionDescription"].ToString());
                    listPositions.Add(position);
                }
            }
            return listPositions;
        }

        public static string GetPositionTitle(int positionId)
        {
            string positionTitle = "";
            using (SqlConnection conn = UtilityDB.GetDBConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT PositionTitle FROM Positions WHERE PositionID = @PositionID", conn);
                cmd.Parameters.AddWithValue("@PositionID", positionId);
                positionTitle = cmd.ExecuteScalar().ToString();
            }
            return positionTitle;
        }
    }
}
