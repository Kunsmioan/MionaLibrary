﻿using System;
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
        UserRepo? _repo;
        public void Add(User user)
        {
            _repo = new();
            _repo.Add(user);
        }

        public void Remove(User user)
        {
            _repo = new();
            _repo.Remove(user);
        }

        public void Update(User user)
        {
            _repo = new();
            _repo.Update(user);
        }
    }
}
