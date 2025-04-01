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
    /// Interaction logic for ExtendOrReturnBook.xaml
    /// </summary>
    public partial class ExtendOrReturnBook : UserControl
    {
        LoanServices _loanServices = new();

        public ExtendOrReturnBook()
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
            if ((sender as Button)?.DataContext is Loan loan)
            {
                // Lấy cửa sổ cha
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow is ManagerWindow mw)
                {
                    User? manager = mw.GetManager();
                    var viewUsersWithLoans = new ViewUsersWithLoans();
                    viewUsersWithLoans.SetBookSelected(loan.Book);
                    viewUsersWithLoans.SetUser(loan.User);

                    // Thay thế nội dung hiện tại bằng BookDetailsControl
                    mw.MainContent.Content = viewUsersWithLoans;
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Extend_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
