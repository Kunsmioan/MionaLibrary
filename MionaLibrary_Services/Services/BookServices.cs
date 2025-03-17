using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;

namespace MionaLibrary_Services.Services
{
    
    public class BookServices
    {
        private BookRepo? _repo;

        public void AddBook(Book book)
        {
            _repo = new();
            _repo.AddBook(book);
        }

        public void UpdateBook(Book book)
        {
            _repo = new();
            _repo.UpdateBook(book);
        }

        public void DeleteBook(Book book)
        {
            _repo = new();
            _repo.DeleteBook(book);
        }   

        public List<Book> GetAllBooks()
        {
            _repo = new();
            List<Book> books = _repo.GetAllBooks();
            return books;
        }
    }
}
