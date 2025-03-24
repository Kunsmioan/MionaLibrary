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

    }
}
