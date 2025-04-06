using MionaLibrary_DAL.Entity;
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

namespace MionaLibrary.BookManagerControls
{
    /// <summary>
    /// Interaction logic for ViewReaderStatistic.xaml
    /// </summary>
    public partial class ViewReaderStatistic : UserControl
    {
        User? reader = new();
        LoanServices? _loanServices = new();
        public ViewReaderStatistic()
        {
            InitializeComponent();
        }
        public void SetUser(User user)
        {
            reader = user;
            loadData(); // Chỉ gọi loadData() một lần sau khi đã set cả book và user
        }

        private void loadData()
        {
            if (reader != null)
            {
                // Ensure that reader is not null before setting the content
                tbUsername.Text = reader.Username.ToString();
                tbfullName.Text = reader.FirstName.ToString() + " " + reader.LastName.ToString();
                //if (reader.Birthday.HasValue)
                //{
                //    tbBirthday.Text = reader.Birthday.Value.ToString("dd/MM/yyyy");
                //}
                //else
                //{
                //    tbBirthday.Text = string.Empty; // Or any other default value if birthday is null
                //}
                //tbGender.Text = reader.Gender.ToString();
                tbPhone.Text = reader.Phone?.ToString() ?? string.Empty;
                
                tbBooksOnLoan.Text = _loanServices.GetBooksCurrentlyBorrowed(reader.Id).ToString();
                tbBooksHaveBorrowed.Text = _loanServices.GetBookBorrowed(reader.Id).ToString();
            }
            else
            {
                //MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadBookOnLoanData();
            LoadBookBorrowingHistoryData();
        }

        private void LoadBookOnLoanData()
        {
            var bookOnLoan_List = _loanServices.GetBooksOnLoanOrOverdueByUserId(reader.Id);

            // Gán dữ liệu vào DataGrid
            BooksOnLoanDataGrid.ItemsSource = bookOnLoan_List;
        }

        private void LoadBookBorrowingHistoryData()
        {
            var bookBorrowingHistory_List = _loanServices.GetLoansReturnByUserId(reader.Id);
            // Gán dữ liệu vào DataGrid
            BooksBorrowingHistoryDataGrid.ItemsSource = bookBorrowingHistory_List;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is ManagerWindow mw)
            {
                User? manager = mw.GetManager();
                //if (reader == null)
                //{
                //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                var readersControl = new ReadersControl();

                //updateBookControl.SetUser(manager);

                // Thay thế nội dung hiện tại bằng BookDetailsControl
                mw.MainContent.Content = readersControl;
            }
        }
    }
}
