using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class StatusRepository
    {
        private readonly HiTechDBEntities dbContext;

        public StatusRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        public IEnumerable<Status> GetAllStatuses()
        {
            return dbContext.Status.ToList();
        }

        #region CRUD Operations
        public void AddStatus(Status status)
        {
            dbContext.Status.Add(status);
            dbContext.SaveChanges();
        }
        public Status GetStatusById(int id)
        {
            return dbContext.Status.Find(id);
        }
        #endregion
    }
}
