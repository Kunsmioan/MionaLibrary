using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.Repository
{

    public class LoanHistoryRepo
    {
        LibraryManagerContext? _context;

        public void AddLoanHistory(LoanHistory loanHistory)
        {
            _context = new LibraryManagerContext();
            _context.LoanHistories.Add(loanHistory);
            _context.SaveChanges();
        }
    }
}
