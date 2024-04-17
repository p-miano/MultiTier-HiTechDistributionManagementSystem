using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class AuthorController
    {
        private readonly AuthorRepository authorRepository;

        public AuthorController()
        {
            authorRepository = new AuthorRepository();
        }

        public IEnumerable<Authors> GetAllAuthors() => authorRepository.GetAllAuthors();
        public int AddAuthor(Authors author)
        {
            authorRepository.AddAuthor(author);
            return author.AuthorID;
        }

        public Authors GetAuthorById(int id) => authorRepository.GetAuthorById(id);
    }
}
