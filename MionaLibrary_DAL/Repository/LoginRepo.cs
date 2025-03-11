using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.Repository
{
    public class LoginRepo
    {
        private LibraryManagerContext _context;

        public User getOne(string username, string password)
        {
            _context = new();
            User? user = _context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user;
        }
    }
}
