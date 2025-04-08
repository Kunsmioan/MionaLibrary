using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using static MionaLibrary_DAL.Repository.BookRepo;

namespace MionaLibrary_Services.Services
{
    
    public class BookServices
    {
       BookRepo? _repo;

        public BookServices()
        {
            _repo = new();
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

        public Book GetBookById(int id)
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

        public List<Book> GetBookWithEarliestCreateDate()
        {
            return _repo.GetBookWithEarliestCreateDate();
        }

        public int GetTotalBooks()
        {
            return _repo.GetTotalBooks();
        }

        public int GetTotalTitleBooks()
        {
            // Calculate the total number of books by summing up the 'quantity' field for each book
            return _repo.GetTotalTitleBooks();
        }

        public List<TopBookDto> GetTopBooks()
        {
            return _repo.GetTopBooks();
        }
    }
}
