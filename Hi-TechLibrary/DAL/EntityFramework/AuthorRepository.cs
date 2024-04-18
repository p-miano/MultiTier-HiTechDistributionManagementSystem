using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class AuthorRepository
    {
        private readonly HiTechDBEntities dbContext;

        public AuthorRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        public IEnumerable<Authors> GetAllAuthors()
        {
            return dbContext.Authors.ToList();
        }

        #region CRUD Operations
        public void AddAuthor(Authors author)
        {
            dbContext.Authors.Add(author);
            dbContext.SaveChanges();
        }
        public Authors GetAuthorById(int id)
        {
            return dbContext.Authors.Find(id);
        }
        #endregion
    }
}
