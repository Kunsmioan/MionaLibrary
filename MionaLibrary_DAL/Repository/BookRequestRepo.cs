using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_DAL.Repository
{
    public class BookRequestRepo
    {
        LibraryManagerContext? _context;

        public void AddRequestBook(BookRequest book)
        {
            _context = new();
            _context.BookRequests.Add(book);
            _context.SaveChanges();
        }

        public void RemoveRequestBook(BookRequest book)
        {
            _context = new();
            _context.BookRequests.Remove(book);
            _context.SaveChanges();
        }

        public void UpdateRequestBook(BookRequest book)
        {
            _context = new();
            _context.BookRequests.Update(book);
            _context.SaveChanges();
        }

        public List<BookRequest> GetAllRequestBooks()
        {
            _context = new();
            var bookRequest = _context.BookRequests.ToList();
            return bookRequest;
        }

        public bool HasPendingRequest(int userId, int bookId)
        {
            _context = new();
            return _context.BookRequests
                .Any(br => br.UserId == userId && br.BookId == bookId && br.Status == "Pending");
        }
    }
}
