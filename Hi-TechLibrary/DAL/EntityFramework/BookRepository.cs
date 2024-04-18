using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hi_TechLibrary.BLL.EntityFramework;
using System.Data.Entity;

namespace Hi_TechLibrary.DAL.EntityFramework
{
    public class BookRepository
    {
        private readonly HiTechDBEntities dbContext;

        public BookRepository()
        {
            dbContext = new HiTechDBEntities();
        }
        public IEnumerable<Books> GetAllBooks()
        {
            return dbContext.Books.Include(b => b.Authors).ToList();
        }

        #region CRUD Operations
        public void AddBook(Books book, List<int> authorIds)
        {
            // Ensure the book is tracked by the context
            dbContext.Books.Add(book);
            // Clear existing authors to avoid duplication issues
            book.Authors.Clear();
            // Add authors to the book based on provided author IDs
            foreach (var authorId in authorIds)
            {
                var author = dbContext.Authors.Find(authorId);
                if (author != null)
                {
                    book.Authors.Add(author);
                }
            }
            // Save all changes to the database
            dbContext.SaveChanges();
        }

        public Books GetBookByISBN(string iSBN)
        {
            return dbContext.Books.Include(b => b.Authors).FirstOrDefault(b => b.ISBN == iSBN);
        }
        public void UpdateBook(Books book, List<int> newAuthorIds)
        {
            // Fetch the book along with its current authors from the database
            Books bookToUpdate = dbContext.Books.Include(b => b.Authors).FirstOrDefault(b => b.ISBN == book.ISBN);
            if (bookToUpdate != null)
            {
                // Update scalar properties
                bookToUpdate.Title = book.Title;
                bookToUpdate.Price = book.Price;
                bookToUpdate.YearPublished = book.YearPublished;
                bookToUpdate.QOH = book.QOH;
                bookToUpdate.CategoryID = book.CategoryID;
                bookToUpdate.PublisherID = book.PublisherID;
                bookToUpdate.StatusID = book.StatusID;
                // Update the authors list
                bookToUpdate.Authors.Clear();  // Remove all current authors to avoid duplication
                // Add new authors based on provided IDs
                foreach (var authorId in newAuthorIds)
                {
                    var author = dbContext.Authors.Find(authorId);
                    if (author != null)
                    {
                        bookToUpdate.Authors.Add(author);
                    }
                }
                // Save the changes in the database
                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Book not found");
            }
        }

        public void DeleteBook(string iSBN)
        {
            Books bookToDelete = dbContext.Books.FirstOrDefault(b => b.ISBN == iSBN);
            if (bookToDelete != null)
            {
                dbContext.Books.Remove(bookToDelete);
                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Book not found");
            }
        }
        #endregion

    }
}
