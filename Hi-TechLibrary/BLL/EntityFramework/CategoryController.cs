using Hi_TechLibrary.DAL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class CategoryController
    {
        private readonly CategoryRepository categoryRepository;

        public CategoryController()
        {
            categoryRepository = new CategoryRepository();
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return categoryRepository.GetAllCategories();
        }
    }
}
