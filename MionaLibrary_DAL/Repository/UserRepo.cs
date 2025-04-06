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
        private readonly LibraryManagerContext _context;

        public UserRepo()
        {
            _context = new LibraryManagerContext();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Remove(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(b => b.Id == id);
        }

        public List<User> GetAll()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                // Log the error if you have logging configured
                throw new InvalidOperationException("Error retrieving users from database", ex);
            }
        }

    }
}
