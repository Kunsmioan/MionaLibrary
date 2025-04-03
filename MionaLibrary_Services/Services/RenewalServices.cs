using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using System;
using System.Collections.Generic;

namespace MionaLibrary_Services.Services
{
    public class RenewalServices
    {
        private readonly RenewalRepo _repo;
        private const int MaxRenewalsPerLoan = 3;

        public RenewalServices()
        {
            _repo = new RenewalRepo();
        }

        public Renewal? GetRenewalById(int id)
        {
            return _repo.GetRenewalById(id);
        }

        public List<Renewal> GetAllRenewals()
        {
            return _repo.GetAllRenewals();
        }

        public List<Renewal> GetRenewalsByUserId(int userId)
        {
            return _repo.GetRenewalsByUserId(userId);
        }

        public int GetRenewalCountForLoan(int loanId)
        {
            return _repo.GetRenewalCountForLoan(loanId);
        }

        public List<Renewal> GetRenewalsByLoanId(int loanId)
        {
            return _repo.GetRenewalsByLoanId(loanId);
        }
        public Renewal RenewLoan(Loan loan, User user)
        {
            return _repo.RenewLoan(loan, user);
        }
    }
}
