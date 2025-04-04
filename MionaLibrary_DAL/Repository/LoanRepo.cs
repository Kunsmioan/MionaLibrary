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

        public Loan GetLoanByUserIdAndBookId(int userId, int bookId)
        {
            // Tìm khoản vay dựa trên UserId và BookId
            Loan? loan = _context.Loans
                .Include(loan => loan.Book) // Bao gồm thông tin sách
                .Include(loan => loan.User) // Bao gồm thông tin người dùng
                .FirstOrDefault(loan => loan.UserId == userId && loan.BookId == bookId);

            // Nếu không tìm thấy, trả về null
            return loan;
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

        public List<Loan> GetBooksOnLoanOrOverdueByBookId(int bookId)
        {
            return _context.Loans
                .Include(loan => loan.Book) // Bao gồm thông tin sách
                .Include(loan => loan.User) // Bao gồm thông tin người dùng
                .Where(loan => (loan.Status == "Borrowing" || loan.Status == "Overdue") 
                    && loan.BookId == bookId) // Lọc theo trạng thái và BookId
                .ToList();
        }

        public List<Loan> GetBooksOnLoanOrOverdueByUserId(int userId)
        {
            return _context.Loans
                .Include(loan => loan.Book) // Bao gồm thông tin sách
                .Include(loan => loan.User) // Bao gồm thông tin người dùng
                .Where(loan => (loan.Status == "Borrowing" || loan.Status == "Overdue")
                    && loan.UserId == userId) // Lọc theo trạng thái và BookId
                .ToList();
        }

        public List<Loan> FilterReadesBorrowingOrOvedue(string searchTerm)
        {
            // Ensure the search term is not null or empty
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Loan>(); // Return an empty list if no search term is provided
            }

            // Normalize the search term for case-insensitive comparison
            string normalizedSearchTerm = searchTerm.Trim().ToLower();

            // Query the database to find loans where the associated user matches the search term
            var filteredLoans = _context.Loans
                .Include(loan => loan.Book) // Include book details
                .Include(loan => loan.User) // Include user details
                .Where(loan => (loan.Status == "Borrowing" || loan.Status == "Overdue") &&
                    loan.User != null && // Ensure the user is not null
                    (
                        loan.User.Username != null && loan.User.Username.ToLower().Contains(normalizedSearchTerm)
                    )
                )
                .ToList();

            return filteredLoans;
        }

        public List<Loan> FilterBooksBorrowingOrOvedue(string searchTerm)
        {
            // Ensure the search term is not null or empty
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Loan>(); // Return an empty list if no search term is provided
            }

            // Normalize the search term for case-insensitive comparison
            string normalizedSearchTerm = searchTerm.Trim().ToLower();

            // Query the database to find loans where the associated user matches the search term
            var filteredLoans = _context.Loans
                .Include(loan => loan.Book) // Include book details
                .Include(loan => loan.User) // Include user details
                .Where(loan => (loan.Status == "Borrowing" || loan.Status == "Overdue") &&
                    loan.Book != null && // Ensure the user is not null
                    (
                        loan.Book.Title != null && loan.Book.Title.ToLower().Contains(normalizedSearchTerm)
                    )
                )
                .ToList();

            return filteredLoans;
        }

    }
}
