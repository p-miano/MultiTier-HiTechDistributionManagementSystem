using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class CategoryRepository
    {
        private readonly HiTechDBEntities dbContext;

        public CategoryRepository()
        {
            dbContext = new HiTechDBEntities();
        }

        // Get all categories from the database
        public IEnumerable<Categories> GetAllCategories()
        {
            return dbContext.Categories.ToList();
        }
    }
}
