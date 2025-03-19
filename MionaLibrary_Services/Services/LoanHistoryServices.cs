using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;

namespace MionaLibrary_Services.Services
{
    public class LoanHistoryServices
    {
        LoanHistoryRepo? _loanHistoryRepo;
        public void AddLoanHistory(LoanHistory loanHistory)
        {
            _loanHistoryRepo = new();
            _loanHistoryRepo.AddLoanHistory(loanHistory);
        }
    }
}
