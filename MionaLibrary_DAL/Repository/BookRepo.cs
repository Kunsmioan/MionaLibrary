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

        public Book GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }   

        public List<Book> GetAllBooks() {
            _context = new();
            List<Book> books = _context.Books.ToList();
            return books;
        }

        public List<Book> GetAllBooksByFilter(string searchType, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return _context.Books.ToList(); // Trả về tất cả nếu không có từ khóa
            }

            var lowerSearchTerm = searchTerm.ToLower();

            switch (searchType.ToLower())
            {
                case "title":
                    return _context.Books
                        .Where(b => b.Title.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                case "author":
                    return _context.Books
                        .Where(b => b.Author.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                case "language":
                    return _context.Books
                        .Where(b => b.Language.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                case "genre":
                    return _context.Books
                        .Where(b => b.Genre.ToLower().Contains(lowerSearchTerm))
                        .ToList();
                default:
                    return _context.Books.ToList(); // Trả về tất cả nếu loại tìm kiếm không hợp lệ
            }
        }
    }
}
