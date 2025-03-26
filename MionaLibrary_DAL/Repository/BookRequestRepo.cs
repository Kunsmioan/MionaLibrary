using Microsoft.EntityFrameworkCore;
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
        LibraryManagerContext? _context = new();

        public void AddRequestBook(BookRequest book)
        {
            _context.BookRequests.Add(book);
            _context.SaveChanges();
        }

        public void RemoveRequestBook(BookRequest book)
        {
            _context.BookRequests.Remove(book);
            _context.SaveChanges();
        }

        public void UpdateRequestBook(BookRequest book)
        {
            _context.BookRequests.Update(book);
            _context.SaveChanges();
        }

        public List<BookRequest> GetAllRequestBooks()
        {
            var bookRequest = _context.BookRequests.ToList();
            return bookRequest;
        }

        public bool HasPendingRequest(int userId, int bookId)
        {
            return _context.BookRequests
                .Any(br => br.UserId == userId && br.BookId == bookId && br.Status == "Pending");
        }

        public List<BookRequest> GetPendingRequests()
        {
            return _context.BookRequests
                .Where(br => br.Status == "Pending") // Chỉ lấy các yêu cầu đang chờ phê duyệt
                .Include(br => br.User)             // Lấy thông tin người dùng
                .Include(br => br.Book)            // Lấy thông tin sách
                .ToList();
        }

        public BookRequest? GetRequestById(int requestId)
        {
            return _context.BookRequests
                .Include(br => br.User) // Lấy thông tin người dùng
                .Include(br => br.Book) // Lấy thông tin sách
                .FirstOrDefault(br => br.RequestId == requestId);
        }
    }
}
