using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.Repository
{
    public class BookRepo
    {
       LibraryManagerContext? _context;

       public void AddBook(Book book)
        {
            _context = new();
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context = new();
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(Book book)
        {
            _context = new();
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public Book? GetBookById(int id)
        {
            _context = new();
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }   

        public List<Book> GetAllBooks() {
            _context = new();
            List<Book> books = _context.Books
                                       .Include(b => b.Genre)
                                       .ToList();
            return books;
        }

        public List<Book> GetAllBooksByFilter(string searchType, string searchTerm)
        {
            _context = new();
            if (string.IsNullOrEmpty(searchTerm))
            {_context = new();
                return GetAllBooks(); // Trả về tất cả nếu không có từ khóa
            }

            var lowerSearchTerm = searchTerm.ToLower();

            switch (searchType.ToLower())
            {
                case "title":
                    return _context.Books.Include(b => b.Genre)
                        .Where(b => b.Title.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                case "author":
                    return _context.Books.Include(b => b.Genre)
                        .Where(b => b.Author.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                case "language":
                    return _context.Books.Include(b => b.Genre)
                        .Where(b => b.Language.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                case "genre":
                    return _context.Books
                         .Include(b => b.Genre)
                         .Where(b => b.Genre != null && b.Genre.Name.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                default:
                    return _context.Books.ToList(); // Trả về tất cả nếu loại tìm kiếm không hợp lệ
            }
        }
    }
}
