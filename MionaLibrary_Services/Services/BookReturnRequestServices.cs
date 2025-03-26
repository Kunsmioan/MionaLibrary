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
    public class BookReturnRequestServices
    {
        BookReturnRequest? _repo = new();


        public void AddBookReturn(ReturnRequest bookReturn)
        {
            _repo.AddBookReturn(bookReturn);
        }

        public void DeleteBookReturn(ReturnRequest bookReturn)
        {
            _repo.DeleteBookReturn(bookReturn);
        }

        public void UpdateBookReturn(ReturnRequest bookReturn)
        {
            _repo.UpdateBookReturn(bookReturn);
        }

        public List<ReturnRequest> GetReturnRequestsByUserId(int userId)
        {
            return _repo.GetReturnRequestsByUserId(userId);
        }

        // Phương thức lấy danh sách yêu cầu trả sách có trạng thái "Pending"
        public List<ReturnRequest> GetPendingReturnRequests()
        {
            return _repo.GetPendingReturnRequests();
        }

        // Phương thức lấy yêu cầu trả sách theo ID
        public ReturnRequest GetReturnRequestById(int returnRequestId)
        {
            return _repo.GetReturnRequestById(returnRequestId);
        }


        public List<ReturnRequest> GetAllReturnRequests()
        {
            return _repo.GetAllReturnRequests();
        }
    }
}
