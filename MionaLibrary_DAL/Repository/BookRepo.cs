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
        private readonly LibraryManagerContext _context;

        public BookRepo()
        {
            _context = new LibraryManagerContext();
        }

        public void AddBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            
            var existingBook = _context.Books
                                     .Include(b => b.Genre)
                                     .Include(b => b.Language)
                                     .FirstOrDefault(b => b.Id == book.Id);

            if (existingBook != null)
            {
                //Lệnh này sao chép tất cả các giá trị từ đối tượng book (được truyền vào) sang đối tượng
                //existingBook (đã lấy từ cơ sở dữ liệu).
                _context.Entry(existingBook).CurrentValues.SetValues(book);

                existingBook.GenreId = book.GenreId;
                existingBook.LanguageId = book.LanguageId;
                _context.SaveChanges();
            }
        }

        public void DeleteBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books
                          .Include(b => b.Genre)
                          .Include(b => b.Language)
                          .FirstOrDefault(b => b.Id == id);
        }   

        public List<Book> GetAllBooks() 
        {
            return _context.Books
                          .Include(b => b.Genre)
                          .Include(b => b.Language)
                          .ToList();
        }

        public int GetTotalBooks()
        {
            // Calculate the total number of books by summing up the 'quantity' field for each book
            return _context.Books
                          .Include(b => b.Genre) // Include related Genre data if needed
                          .Include(b => b.Language) // Include related Language data if needed
                          .Sum(b => b.Quantity); // Sum up the 'Quantity' field for all books
        }

        public int GetTotalTitleBooks()
        {
            // Calculate the total number of books by summing up the 'quantity' field for each book
            return _context.Books
                          .Include(b => b.Genre) // Include related Genre data if needed
                          .Include(b => b.Language) // Include related Language data if needed
                          .Count(b => b.Quantity > 0); // Count the number of books with quantity greater than 0

        }

        public List<Book> GetAllBooksByFilter(string searchType, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || searchType == "--- All ---")
            {
                return GetAllBooks();
            }

            var lowerSearchTerm = searchTerm.ToLower();
            var query = _context.Books
                               .Include(b => b.Genre)
                               .Include(b => b.Language);

            return searchType.ToLower() switch
            {
                "title" => query.Where(b => b.Title.ToLower().Contains(lowerSearchTerm)).ToList(),
                "author" => query.Where(b => b.Author.ToLower().Contains(lowerSearchTerm)).ToList(),
                "language" => query.Where(b => b.Language != null && b.Language.Name.ToLower().Contains(lowerSearchTerm)).ToList(),
                "genre" => query.Where(b => b.Genre != null && b.Genre.Name.ToLower().Contains(lowerSearchTerm)).ToList(),
                _ => GetAllBooks()
            };
        }

        public List<Book> GetBookWithEarliestCreateDate()
        {
            return _context.Books
               .Include(b => b.Genre)
               .Include(b => b.Language)
               .OrderByDescending(b => b.CreateDate).Take(5)
               .ToList();
        }
    }
}
