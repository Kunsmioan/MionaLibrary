using MionaLibrary.BookControls;
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

namespace MionaLibrary.BookManagerControls
{
    /// <summary>
    /// Interaction logic for ViewBooksOnLoan.xaml
    /// </summary>
    public partial class ViewBooksOnLoan : UserControl
    {
        LoanServices _loanServices = new();
        User? manager;

        public ViewBooksOnLoan()
        {
            InitializeComponent();
            LoadBooksOnLoanData();
        }

        private void LoadBooksOnLoanData()
        {
            var bookOnLoan_List = _loanServices.GetBooksOnLoanOrOverdue();

            // Gán dữ liệu vào DataGrid
            BooksOnLoanDataGrid.ItemsSource = bookOnLoan_List;
        }

        private void BookDetailsAndReaderBorrowing_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Loan loan)
            {
                // Lấy cửa sổ cha
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow is ManagerWindow mw)
                {
                    User? manager = mw.GetManager();
                    var bookDetailsAndReaderBorrowing = new BookDetailsAndReaderBorrowing();
                    bookDetailsAndReaderBorrowing.SetBookSelected(loan.Book);
                    //bookDetailsAndReaderBorrowing.SetLoanSelected(loan);
                    bookDetailsAndReaderBorrowing.SetUser(manager);

                    // Thay thế nội dung hiện tại bằng BookDetailsControl
                    mw.MainContent.Content = bookDetailsAndReaderBorrowing;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cửa sổ ReaderWindow.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Không thể lấy thông tin sách từ nút.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReaderDetailsAndBooksOnloan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
