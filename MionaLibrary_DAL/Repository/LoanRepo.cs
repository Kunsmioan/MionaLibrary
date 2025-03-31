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
    public class LoanRepo
    {
        LibraryManagerContext? _context = new();

        public void AddLoan(Loan loan)
        {
            _context.Loans.Add(loan);
            _context.SaveChanges();
        }

        public void UpdateLoan(Loan loan)
        {
            _context.Loans.Update(loan);
            _context.SaveChanges();
        }

        public void DeleteLoan(Loan loan)
        {
            _context.Loans.Remove(loan);
            _context.SaveChanges();
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = _context.Loans.ToList();
            return loans;
        }

        public Book getBookById(int bookId)
        {
            Book? bookSelected = _context.Books
                                .Include(b => b.Genre)
                                .FirstOrDefault(book => book.Id == bookId);

            return bookSelected;
        }

        public List<Loan> GetLoanByUser(int userId)
        {
            List<Loan> loans = _context.Loans
                                       .Include(loan => loan.Book)
                                       .Where(loan => loan.UserId == userId && (loan.Status == "Borrowing" || loan.Status == "Overdue"))
                                       .ToList();
            return loans;
        }

        public List<Loan> GetLoanReturnByUser(int userId)
        {
            List<Loan> loans = _context.Loans
                                       .Include(loan => loan.Book)
                                       .Where(loan => loan.UserId == userId && (loan.Status == "Returned"))
                                       .ToList();
            return loans;
        }

        public bool IsBookBorrowedByUser(int bookId, int userId)
        {
            // Kiểm tra xem _context có null hay không
            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }

            // Truy vấn cơ sở dữ liệu để kiểm tra
            var loan = _context.Loans
                .FirstOrDefault(l => l.BookId == bookId && l.UserId == userId && l.Status == "Borrowing");

            return loan != null; // Trả về true nếu tìm thấy khoản vay
        }

        public void UpdateOverdueLoans()
        {
            // Lấy ngày hiện tại (chỉ lấy phần ngày, không tính thời gian)
            DateTime today = DateTime.Today;

            // Tìm các khoản vay đang ở trạng thái "Borrowing" và đã quá hạn
            var overdueLoans = _context.Loans
                .Where(l => l.Status == "Borrowing" && l.DueDate < today)
                .ToList();

            // Cập nhật trạng thái thành "Overdue"
            foreach (var loan in overdueLoans)
            {
                loan.Status = "Overdue";
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
        }

        public List<Loan> GetBooksOnLoanOrOverdue()
        {
            return _context.Loans
                .Include(loan => loan.Book) // Bao gồm thông tin sách
                .Include(loan => loan.User) // Bao gồm thông tin người dùng
                .Where(loan => loan.Status == "Borrowing" || loan.Status == "Overdue") // Chỉ lấy sách đang mượn hoặc quá hạn
                .ToList();
        }

    }
}
