using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.Repository
{
    public class RegisterRepo
    {
        private LibraryManagerContext? _context;
        public void Add(User user)
        {
                _context = new LibraryManagerContext(); // Initialize context

                _context.Users.Add(user); // Add user to the database
                _context.SaveChanges(); // Save changes to the database

        }
    }
}
