using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.Repository
{
    public class UserRepo
    {
        LibraryManagerContext? _context;
        public void Add(User user)
        {
            _context = new();
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Remove(User user)
        {
            _context = new();
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public void Update(User user)
        {
            _context = new();
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public List<User> GetAll()
        {
            {
                _context = new();
                List<User> users = _context.Users.ToList();
                return users;
            }
        }
    }
}
