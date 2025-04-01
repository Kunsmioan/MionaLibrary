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
            MessageBox.Show("The request has been approved!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
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
            MessageBox.Show("The request has been rejected!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
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

        //private void LoadReturnRequests()
        //{
        //    var returnRequests = _bookReturnRequestServices.GetPendingReturnRequests();
        //    ReturnRequestDataGrid.ItemsSource = returnRequests;
        //}

        //private void ApproveReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // Lấy ID của yêu cầu trả sách từ nút "Approve"
        //        var returnRequestId = (int)((Button)sender).Tag;

        //        // Cập nhật trạng thái yêu cầu trả sách thành "Approved"
        //        UpdateReturnRequestStatus(returnRequestId, "Approved");

        //        // Lấy thông tin yêu cầu trả sách
        //        var returnRequest = _bookReturnRequestServices.GetReturnRequestById(returnRequestId);
        //        if (returnRequest == null)
        //        {
        //            MessageBox.Show("Không tìm thấy yêu cầu trả sách.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        // Lấy thông tin mượn và sách liên quan
        //        var loan = returnRequest.Loan;
        //        var book = returnRequest.Book;

        //        if (loan == null || book == null)
        //        {
        //            MessageBox.Show("Thông tin khoản vay hoặc sách không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        // Cập nhật trạng thái khoản vay
        //        loan.ReturnDate = DateTime.Now;
        //        loan.Status = "Returned";
        //        _loanServices.UpdateLoan(loan);

        //        // Tăng số lượng sách lên 1
        //        book.Quantity += 1;

        //        // Cập nhật trạng thái IsAvailable nếu số lượng sách > 0
        //        if (book.Quantity > 0)
        //        {
        //            book.IsAvailable = true;
        //        }

        //        // Lưu cập nhật sách vào cơ sở dữ liệu
        //        _bookServices.UpdateBook(book);

        //        // Hiển thị thông báo thành công
        //        MessageBox.Show("Yêu cầu trả sách đã được phê duyệt!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        //        // Làm mới danh sách
        //        LoadReturnRequests();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi phê duyệt yêu cầu trả sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void RejectReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // Lấy ID của yêu cầu trả sách từ nút "Reject"
        //        var returnRequestId = (int)((Button)sender).Tag;

        //        // Cập nhật trạng thái yêu cầu trả sách thành "Rejected"
        //        UpdateReturnRequestStatus(returnRequestId, "Rejected");

        //        // Hiển thị thông báo thành công
        //        MessageBox.Show("Yêu cầu trả sách đã bị từ chối!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        //        // Làm mới danh sách
        //        LoadReturnRequests();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi từ chối yêu cầu trả sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void UpdateReturnRequestStatus(int returnRequestId, string status)
        //{
        //    var returnRequest = _bookReturnRequestServices.GetReturnRequestById(returnRequestId);
        //    if (returnRequest != null)
        //    {
        //        returnRequest.Status = status;

        //        // Lưu thay đổi vào cơ sở dữ liệu
        //        _bookReturnRequestServices.UpdateBookReturn(returnRequest);
        //    }
        //}
    }
}
