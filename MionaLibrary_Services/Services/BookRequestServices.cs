using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_Services.Services
{
    public class BookRequestServices
    {
        BookRequestRepo? _repo = new();

        public void AddBookRequest(BookRequest br)
        {
            _repo.AddRequestBook(br);
        }

        public void RemoveBookRequest(BookRequest br)
        {
            _repo.RemoveRequestBook(br);
        }

        public void UpdateBookRequest(BookRequest br)
        {
            _repo.UpdateRequestBook(br);
        }

        public List<BookRequest> GetBookRequests()
        {
            return _repo.GetAllRequestBooks();
        }

        public bool HasPendingRequest(int userId, int bookId)
        {
            return _repo.HasPendingRequest(userId, bookId);
        }

        public List<BookRequest> GetPendingRequests()
        {
            return _repo.GetPendingRequests();
        }

        public BookRequest? GetRequestById(int requestId)
        {
            return _repo.GetRequestById(requestId);
        }
    }
}
