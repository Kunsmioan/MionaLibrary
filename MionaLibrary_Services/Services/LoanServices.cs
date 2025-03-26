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
        LoanRepo? _repo;

        public void AddLoan( Loan loan)
        {
            _repo = new();
            _repo.AddLoan(loan);
        }

        public void UpdateLoan(Loan loan)
        {
            _repo = new();
            _repo.UpdateLoan(loan);
        }

        public void DeleteLoan(Loan loan)
        {
            _repo = new();
            _repo.DeleteLoan(loan);
        }

        public List<Loan> GetAllLoans()
        {
            _repo = new();
            return _repo.GetAllLoans();
        }

        public List<Loan> GetLoansByUserId(int id)
        {
            _repo = new();
            return _repo.GetLoanByUser(id);
        }

        public List<Loan> GetLoansReturnByUserId(int id)
        {
            _repo = new();
            return _repo.GetLoanReturnByUser(id);
        }

        public Book bookSelectedById(int bookId)
        {
            _repo = new();
            return _repo.getBookById(bookId);
        }

        public bool IsBookBorrowedByUser(int bookId, int userId)
        {
            _repo = new();
            return _repo.IsBookBorrowedByUser(bookId, userId);
        }

    }
}
