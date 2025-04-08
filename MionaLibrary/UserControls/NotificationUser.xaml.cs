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
        BookRequestServices? _bookRequestServices = new();
        LoanServices? _loanServices = new();    
        User? reader;

        public NotificationUser()
        {
            InitializeComponent();
        }

        public void setReader(User user)
        {
            reader = user;
            loadData();
        }

        public void loadData()
        { 
            if(reader != null)
            {
            LoadUserRequests();
            LoadBookOverdueOrGonnaBeOverdue();
            }
           
        }

        private void LoadUserRequests()
        {
            // Lấy danh sách yêu cầu của người dùng hiện tại
            var userRequests = _bookRequestServices.GetUserRequests(reader.Id);

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

        public void LoadBookOverdueOrGonnaBeOverdue()
        {
            var userOverdueBook = _loanServices.GetBooksOverdueOrGonnaBeOverdueByUserId(reader.Id);

            //Cập nhật thông báo và màu sắc cho từng yêu cầu
            foreach (var loan in userOverdueBook)
            {
                if (loan.Status == "Overdue")
                {
                    loan.Announce = "Your book is overdue. Please return it as soon as possible!";
                }
                else if (loan.Status == "Borrowing")
                {
                    loan.Announce = "Your book is going to be overdue soon. Please return it!";
                }
            }
            // Gán dữ liệu vào DataGrid
            UserOverdueBookDataGrid.ItemsSource = userOverdueBook;
        }

    }
}
