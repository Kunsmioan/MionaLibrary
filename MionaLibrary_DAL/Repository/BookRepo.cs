using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.Repository
{

    public class BookRepo
    {
       private LibraryManagerContext _context;

       public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public List<Book> GetAllBook() { 
            List<Book> books = _context.Books.ToList();
            return books;
        }
    }
}
