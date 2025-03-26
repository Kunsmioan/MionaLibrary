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
    public class BookReturnRequest
    {
        LibraryManagerContext? _context = new();

        public void AddBookReturn(ReturnRequest bookReturn)
        {
            _context.ReturnRequests.Add(bookReturn);
            _context.SaveChanges();
        }

        public void DeleteBookReturn(ReturnRequest bookReturn)
        {
            _context.ReturnRequests.Remove(bookReturn);
            _context.SaveChanges();
        }

        public void UpdateBookReturn(ReturnRequest bookReturn)
        {
            _context.ReturnRequests.Update(bookReturn);
            _context.SaveChanges();
        }

        public List<ReturnRequest> GetReturnRequestsByUserId(int userId)
        {
            return _context.ReturnRequests
                           .Include(rr => rr.Loan) // Liên kết với bảng Loans
                           .Include(rr => rr.User)
                           .Include(rr => rr.Book) // Liên kết với bảng Books
                           .Where(rr => rr.Loan.UserId == userId)
                           .ToList();
        }

        // Phương thức lấy danh sách yêu cầu trả sách có trạng thái "Pending"
        public List<ReturnRequest> GetPendingReturnRequests()
        {
            return _context.ReturnRequests
                           .Include(rr => rr.Loan) // Liên kết với bảng Loans
                           .Include(rr => rr.Book) // Liên kết với bảng Books
                           .Include(rr => rr.User)
                           .Where(rr => rr.Status == "Pending") // Chỉ lấy yêu cầu có trạng thái "Pending"
                           .ToList();
        }

        // Phương thức lấy yêu cầu trả sách theo ID
        public ReturnRequest? GetReturnRequestById(int returnRequestId)
        {
            return _context.ReturnRequests
                           .Include(rr => rr.Loan)
                           .Include(rr => rr.Book)
                           .Include(rr => rr.User)
                           .FirstOrDefault(rr => rr.ReturnRequestId == returnRequestId);
        }


        public List<ReturnRequest> GetAllReturnRequests()
        {
            return _context.ReturnRequests.ToList();
        }
    }
}
