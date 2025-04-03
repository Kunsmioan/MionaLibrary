using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MionaLibrary_DAL.Repository
{
    public class RenewalRepo
    {
        private readonly LibraryManagerContext _context;

        public RenewalRepo()
        {
            _context = new LibraryManagerContext();
        }

        public void AddRenewal(Renewal renewal)
        {
            _context.Renewals.Add(renewal);
            _context.SaveChanges();
        }

        public void UpdateRenewal(Renewal renewal)
        {
            _context.Renewals.Update(renewal);
            _context.SaveChanges();
        }

        public void DeleteRenewal(Renewal renewal)
        {
            _context.Renewals.Remove(renewal);
            _context.SaveChanges();
        }

        public Renewal? GetRenewalById(int id)
        {
            return _context.Renewals
                          .Include(r => r.Loan)
                          .Include(r => r.User)
                          .FirstOrDefault(r => r.Id == id);
        }

        public List<Renewal> GetAllRenewals()
        {
            return _context.Renewals
                          .Include(r => r.Loan)
                          .Include(r => r.User)
                          .ToList();
        }

        public List<Renewal> GetRenewalsByUserId(int userId)
        {
            return _context.Renewals
                          .Include(r => r.Loan)
                            .ThenInclude(l => l.Book)
                          .Include(r => r.User)
                          .Where(r => r.UserId == userId)
                          .OrderByDescending(r => r.RenewalDate)
                          .ToList();
        }

        public int GetRenewalCountForLoan(int loanId)
        {
            return _context.Renewals.Count(r => r.LoanId == loanId);
        }

        public List<Renewal> GetRenewalsByLoanId(int loanId)
        {
            return _context.Renewals
                          .Include(r => r.Loan)
                          .Include(r => r.User)
                          .Where(r => r.LoanId == loanId)
                          .OrderByDescending(r => r.RenewalDate)
                          .ToList();
        }

        public Renewal RenewLoan(Loan loan, User user)
        {
            // Tạo bản ghi gia hạn mới
            var renewal = new Renewal
            {
                LoanId = loan.Id,
                RenewalDate = DateTime.Now,
                NewDueDate = loan.DueDate.AddDays(7), // Gia hạn thêm 7 ngày
                UserId = user.Id
            };

            // Thêm bản ghi vào bảng Renewals
            _context.Renewals.Add(renewal);
            _context.SaveChanges();

            return renewal;
        }
    }
}
