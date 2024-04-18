using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class PublisherRepository
    {
        private readonly HiTechDBEntities dbContext;

        public PublisherRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        public IEnumerable<Publishers> GetAllPublishers()
        {
            return dbContext.Publishers.ToList();
        }

        #region CRUD Operations
        public void AddPublisher(Publishers publisher)
        {
            dbContext.Publishers.Add(publisher);
            dbContext.SaveChanges();
        }
        public Publishers GetPublisherById(int id)
        {
            return dbContext.Publishers.Find(id);
        }
        public Publishers GetPublishByName(string name)
        {
            return dbContext.Publishers.FirstOrDefault(p => p.Name == name);
        }
        #endregion
    }
}
