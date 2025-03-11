using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;

namespace MionaLibrary_Services.Services
{
    public class LoginServices
    {
        LoginRepo _repo;

        public User? Authen(string username, string password)
        {
            _repo = new();
            User? user = _repo.getOne(username, password);
            return user;
        }


    }
}
