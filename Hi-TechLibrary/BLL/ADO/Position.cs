using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.ADO;

namespace Hi_TechLibrary.BLL.ADO
{
    public class Position
    {
        public int PositionID { get; }
        public string PositionTitle { get; set; }
        public string PositionDescription { get; set; }

        // Database will generate PositionID
        public Position()
        {
            PositionTitle = string.Empty;
            PositionDescription = string.Empty;
        }

        // Parameterized constructor for new position creation. Database will generate PositionID.
        public Position(string title, string description)
        {
            PositionTitle = title;
            PositionDescription = description;
        }

        // Overloaded constructor for retrieving an existing position from the database.
        public Position(int positionID, string title, string description)
        {
            PositionID = positionID;
            PositionTitle = title;
            PositionDescription = description;
        }

        // Method to get all positions from the database
        public List<Position> GetAllPositions()
        {
            return PositionDB.GetAllRecords();
        }

        // Method to get the position title from the id
        public string GetPositionById(int positionId)
        {
            return PositionDB.GetPositionTitle(positionId);
        }
    }
}
