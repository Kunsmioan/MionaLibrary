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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the visibility of the placeholder text
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
            else
            {
                PlaceholderText.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Thực hiện tìm kiếm
                SearchButton_Click(sender, e);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy từ khóa tìm kiếm từ TextBox
            var searchTerm = SearchTextBox.Text.Trim();

            // Kiểm tra xem có từ khóa tìm kiếm không
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Nếu không có từ khóa tìm kiếm, hiển thị thông báo trong TextBlock
                MessageTextBlock.Text = "Please enter a search term.";
                MessageTextBlock.Visibility = Visibility.Visible;

                // Clear the DataGrid
                BooksOnLoanDataGrid.ItemsSource = null;
                return;
            }

            // Nếu có từ khóa tìm kiếm, thực hiện tìm kiếm
            var filteredBooks = _loanServices.FilterReadesBorrowingOrOvedue(searchTerm);

            // Kiểm tra nếu không có kết quả tìm kiếm
            if (filteredBooks == null || !filteredBooks.Any())
            {
                // Hiển thị thông báo "Reader not found" trong TextBlock
                MessageTextBlock.Text = "Book not found.";
                MessageTextBlock.Visibility = Visibility.Visible;

                // Clear the DataGrid
                BooksOnLoanDataGrid.ItemsSource = null;
            }
            else
            {
                // Hide the message TextBlock
                MessageTextBlock.Visibility = Visibility.Collapsed;

                // Hiển thị kết quả trong DataGrid
                BooksOnLoanDataGrid.ItemsSource = filteredBooks;
            }
        }

        private void ReaderDetailsAndBooksOnloan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
