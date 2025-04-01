using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;

namespace MionaLibrary_Services.Services
{
    public class UserServices
    {
        UserRepo? _repo = new();
        public void Add(User user)
        {
            _repo.Add(user);
        }

        public void Remove(User user)
        {
            _repo.Remove(user);
        }

        public void Update(User user)
        {
            _repo.Update(user);
        }

        public List<User> GetAll()
        {
            return _repo.GetAll();
        }

        public User? GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
}
