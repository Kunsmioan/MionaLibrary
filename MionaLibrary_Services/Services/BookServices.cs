using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;

namespace MionaLibrary_Services.Services
{
    
    public class BookServices
    {
       private readonly BookRepo _repo;

        public BookServices()
        {
            _repo = new BookRepo();
        }

        public void AddBook(Book book)
        {
            _repo.AddBook(book);
        }

        public void UpdateBook(Book book)
        {
            _repo.UpdateBook(book);
        }

        public void DeleteBook(Book book)
        {
            _repo.DeleteBook(book);
        }   

        public Book? GetBookById(int id)
        {
            return _repo.GetBookById(id);
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = _repo.GetAllBooks();
            return books;
        }

        public List<Book> GetAllBooksByFilter(string searchType, string searchTerm)
        {
            return _repo.GetAllBooksByFilter(searchType, searchTerm);
        }
    }
}
