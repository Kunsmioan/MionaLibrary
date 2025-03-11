using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;

namespace MionaLibrary_Services.Services
{
    public class UserServices
    {
        RegisterRepo? _repo;
        public void Add(User user)
        {
            _repo = new();
            _repo.Add(user);
        }
    }
}
