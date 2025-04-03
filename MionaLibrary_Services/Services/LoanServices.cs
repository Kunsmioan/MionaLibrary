using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_Services.Services
{
    
    public class LoanServices
    {
        LoanRepo? _repo = new();

        public void AddLoan( Loan loan)
        {
            _repo.AddLoan(loan);
        }

        public void UpdateLoan(Loan loan)
        {
            _repo.UpdateLoan(loan);
        }

        public void DeleteLoan(Loan loan)
        {
            _repo.DeleteLoan(loan);
        }

        public Loan GetLoanByUserIdAndBookId(int userId, int bookId)
        {
            return _repo.GetLoanByUserIdAndBookId(userId,bookId);
        }

        public List<Loan> GetAllLoans()
        {
            return _repo.GetAllLoans();
        }

        public List<Loan> GetLoansByUserId(int id)
        {
            return _repo.GetLoanByUser(id);
        }

        public List<Loan> GetLoansReturnByUserId(int id)
        {
            return _repo.GetLoanReturnByUser(id);
        }

        public Book bookSelectedById(int bookId)
        {
            return _repo.getBookById(bookId);
        }

        public bool IsBookBorrowedByUser(int bookId, int userId)
        {
            return _repo.IsBookBorrowedByUser(bookId, userId);
        }

        public void UpdateOverdueLoans()
        {
            _repo.UpdateOverdueLoans(); 
        }

        public List<Loan> GetBooksOnLoanOrOverdue()
        {
            return _repo.GetBooksOnLoanOrOverdue();
        }

        public List<Loan> GetBooksOnLoanOrOverdueByBookId(int bookId)
        {
            return _repo.GetBooksOnLoanOrOverdueByBookId(bookId);
        }

        public List<Loan> GetBooksOnLoanOrOverdueByUserId(int userId)
        {
            return _repo.GetBooksOnLoanOrOverdueByUserId(userId);
        }

    }
}
