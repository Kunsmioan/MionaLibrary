using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using MionaLibrary_Services.Services;
using System;
using System.Collections.Generic;
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

namespace MionaLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for NotificationUser.xaml
    /// </summary>
    public partial class NotificationUser : UserControl
    {
        BookRequestServices? _bookRequestServices =new();
        BookReturnRequestServices? _bookReturnRequestServices = new();
        User? reader;

        public NotificationUser()
        {
            InitializeComponent();
        }

        public void setReader(User? user)
        {
            reader = user;
            LoadUserRequests(reader.Id);
            LoadReturnRequests(reader.Id);
        }

        private void LoadUserRequests(int userId)
        {
            // Lấy danh sách yêu cầu của người dùng hiện tại
            var userRequests = _bookRequestServices.GetUserRequests(userId);

            // Cập nhật thông báo và màu sắc cho từng yêu cầu
            foreach (var request in userRequests)
            {
                switch (request.Status)
                {
                    case "Pending":
                        request.Announce = "Your request is pending approval.";
                        break;
                    case "Approved":
                        request.Announce = "You have successfully borrowed the book. Please pick it up at the counter!";
                        break;
                    case "Rejected":
                        request.Announce = "Your request has been rejected.";
                        break;
                    default:
                        request.Announce = "Status is undefined.";
                        break;
                }
            }

            // Gán dữ liệu vào DataGrid
            UserRequestDataGrid.ItemsSource = userRequests;
        }

        private void LoadReturnRequests(int userId)
        {
            try
            {
                // Lấy danh sách yêu cầu trả sách của người dùng hiện tại
                var returnRequests = _bookReturnRequestServices.GetReturnRequestsByUserId(userId);

                // Cập nhật thông báo và màu sắc cho từng yêu cầu
                foreach (var request in returnRequests)
                {
                    switch (request.Status)
                    {
                        case "Pending":
                            request.Announce = "Your return request is pending approval.";
                            break;
                        case "Approved":
                            request.Announce = "Your return request has been approved. Thank you!";
                            break;
                        case "Rejected":
                            request.Announce = "Your return request has been rejected. Please contact the librarian.";
                            break;
                        default:
                            request.Announce = "Status is undefined.";
                            break;
                    }
                }

                // Gán dữ liệu vào DataGrid
                UserReturnBookDataGrid.ItemsSource = returnRequests;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Error loading return requests: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
