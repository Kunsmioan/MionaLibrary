using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MionaLibrary.ManagerControls
{
    /// <summary>
    /// Interaction logic for NotificationManager.xaml
    /// </summary>
    public partial class NotificationManager : UserControl
    {
        BookRequestServices _bookRequestServices = new();
        LoanServices _loanServices = new();
        BookServices _bookServices = new();
        BookRequest? request = new();

        public NotificationManager()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = _bookRequestServices.GetPendingRequests();
            RequestDataGrid.ItemsSource = requests;
        }

        //chua dc chay
        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            var requestId = (int)((Button)sender).Tag;
            UpdateRequestStatus(requestId, "Approved");
            MessageBox.Show("Yêu cầu đã được phê duyệt!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            _loanServices = new();

            //book, user
            var request = _bookRequestServices.GetRequestById(requestId);

            var reader = request.User;
            var bookSelected = request.Book;

            //datetime to check
            string dateString = "16-3-2025";
            string format = "d-M-yyyy";
            DateTime borrowDate = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            // Lưu lịch sử mượn sách
            Loan loan = new()
            {
                BookId = bookSelected.Id,
                UserId = reader.Id,
                //DueDate = borrowDate.AddDays(7),
                //BorrowDate = borrowDate,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = null,
                Status = "Borrowing"
            };
            _loanServices.AddLoan(loan);

            // Giảm số lượng sách đi 1
            bookSelected.Quantity -= 1;

            // Cập nhật trạng thái IsAvailable nếu số lượng sách bằng 0
            if (bookSelected.Quantity == 0)
            {
                bookSelected.IsAvailable = false;
            }

            // Lưu cập nhật sách vào cơ sở dữ liệu
            _bookServices = new();
            _bookServices.UpdateBook(bookSelected);

            LoadRequests(); // Cập nhật lại danh sách
        }

        //chua dc chay
        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            var requestId = (int)((Button)sender).Tag;
            UpdateRequestStatus(requestId, "Rejected");
            MessageBox.Show("Yêu cầu đã bị từ chối!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadRequests(); // Cập nhật lại danh sách
        }

        private void UpdateRequestStatus(int requestId, string status)
        {
            request = _bookRequestServices.GetPendingRequests().FirstOrDefault(br => br.RequestId == requestId);
            if (request != null)
            {
                request.Status = status;
                // Lưu thay đổi vào cơ sở dữ liệu
                _bookRequestServices.UpdateBookRequest(request);
                LoadRequests();
            }
        }
    }
}
