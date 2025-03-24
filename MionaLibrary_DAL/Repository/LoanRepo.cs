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
        LibraryManagerContext? _context;

        public void AddLoan(Loan loan)
        {
            _context = new();
            _context.Loans.Add(loan);
            _context.SaveChanges();
        }

        public void UpdateLoan(Loan loan)
        {
            _context = new();
            _context.Loans.Update(loan);
            _context.SaveChanges();
        }

        public void DeleteLoan(Loan loan)
        {
            _context = new();
            _context.Loans.Remove(loan);
            _context.SaveChanges();
        }   

        public List<Loan> GetAllLoans()
        {
            _context = new();
            List<Loan> loans = _context.Loans.ToList();
            return loans;
        }

        public List<Loan> GetLoanByUser(int userId)
        {
            _context = new();
            List<Loan> loans = _context.Loans
                                       .Include(loan => loan.Book)
                                       .Where(loan => loan.UserId == userId)
                                       .ToList();
            return loans;
        }

        public bool IsBookBorrowedByUser(int bookId, int userId)
        {
            _context= new();
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

    }
}
