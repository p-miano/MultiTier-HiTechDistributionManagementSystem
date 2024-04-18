using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.DAL.EntityFramework;

namespace Hi_TechLibrary.BLL.EntityFramework
{
    public class BookController
    {
        private readonly BookRepository bookRepository;
        public BookController()
        {
            bookRepository = new BookRepository();
        }
        public IEnumerable<Books> GetAllBooks() => bookRepository.GetAllBooks();
        public void AddBook(Books book, List<int> authorIds)
        {
            bookRepository.AddBook(book, authorIds);
        }
        public Books GetBookByISBN(string iSBN)
        {
            return bookRepository.GetBookByISBN(iSBN);
        }
        public void UpdateBook(Books book, List<int> newAuthorIds)
        {
            bookRepository.UpdateBook(book, newAuthorIds);
        }
        public void DeleteBook(string iSBN)
        {
            bookRepository.DeleteBook(iSBN);
        }
        public bool Exists(string iSBN)
        {
            return bookRepository.GetAllBooks().Any(b => b.ISBN.Equals(iSBN, StringComparison.OrdinalIgnoreCase));
        }
    }
}
