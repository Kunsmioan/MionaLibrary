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
        User? reader;

        public NotificationUser()
        {
            InitializeComponent();
        }

        public void setReader(User? user)
        {
            reader = user;
            LoadUserRequests(reader.Id);
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
                        request.Announce = "Yêu cầu đang chờ phê duyệt.";
                        break;
                    case "Approved":
                        request.Announce = "Bạn đã mượn sách thành công. Vui lòng nhận sách tại quầy!";
                        break;
                    case "Rejected":
                        request.Announce = "Yêu cầu không được chấp nhận.";
                        break;
                    default:
                        request.Announce = "Trạng thái không xác định.";
                        break;
                }
            }

            // Gán dữ liệu vào DataGrid
            UserRequestDataGrid.ItemsSource = userRequests;
        }

    }
}
