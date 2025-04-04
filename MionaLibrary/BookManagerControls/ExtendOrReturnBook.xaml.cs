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
        Book? bookSelected = new();
        private readonly BookServices _bookServices = new();
        private readonly RenewalServices _renewalServices = new();
        private readonly LoanServices _loanServices = new();
        User? reader = new();
        Loan? loanSelected = new();

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
                MessageTextBlock.Text = "Reader not found.";
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

        private void Extend_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if ((sender as Button)?.DataContext is Loan loan)
                {
                    // Lấy thông tin khoản vay từ DataContext của nút
                    loanSelected = loan;
                    // Lấy thông tin người đọc từ DataContext của nút
                    reader = loan.User;
                    // Lấy thông tin sách từ DataContext của nút
                    bookSelected = loan.Book;
                    // Kiểm tra xem các đối tượng cần thiết đã được tải chưa
                    if (loanSelected == null || reader == null || bookSelected == null)
                    {
                        MessageBox.Show("Missing required information. Please ensure all details are loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Kiểm tra số lần gia hạn còn lại
                    int remainingRenewals = loanSelected.RenewalCount; // Số lần gia hạn còn lại
                    if (remainingRenewals <= 0)
                    {
                        MessageBox.Show("This loan has reached its maximum number of renewals.", "Cannot Renew", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Kiểm tra trạng thái khoản vay
                    if (loanSelected.Status != "Borrowing")
                    {
                        MessageBox.Show("This loan cannot be renewed. It may be overdue or already returned.", "Cannot Renew", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Kiểm tra xem có còn đủ 3 ngày trước ngày hạn không
                    DateTime currentDate = DateTime.Now.Date; // Ngày hiện tại (không tính giờ/phút/giây)
                    DateTime dueDate = loanSelected.DueDate.Date; // Ngày hạn (không tính giờ/phút/giây)
                    int daysUntilDue = (dueDate - currentDate).Days;

                    if (daysUntilDue > 3)
                    {
                        MessageBox.Show("You can only extend the loan within 3 days before the due date.", "Cannot Renew", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Thực hiện gia hạn
                    var renewal = _renewalServices.RenewLoan(loanSelected, reader); // Gọi dịch vụ gia hạn
                    if (renewal == null)
                    {
                        MessageBox.Show("Failed to renew the loan. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Cập nhật thông tin khoản vay
                    loanSelected.DueDate = renewal.NewDueDate;
                    loanSelected.RenewalCount -= 1; // Giảm số lần gia hạn còn lại
                    _loanServices.UpdateLoan(loanSelected); // Lưu thay đổi vào cơ sở dữ liệu

                    // Thông báo thành công
                    MessageBox.Show($"The loan has been extended. New due date: {renewal.NewDueDate:d}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    SearchButton_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Loan loan)
            {
                // Lấy thông tin khoản vay từ DataContext của nút
                loanSelected = loan;
                // Lấy thông tin người đọc từ DataContext của nút
                reader = loan.User;
                // Lấy thông tin sách từ DataContext của nút
                bookSelected = loan.Book;
                //Cập nhật trạng thái khoản vay
                loan.ReturnDate = DateTime.Now;
                loan.Status = "Returned";
                loan.RenewalCount = 3;
                _loanServices.UpdateLoan(loan);

                // Tăng số lượng sách lên 1
                bookSelected.Quantity += 1;

                // Cập nhật trạng thái IsAvailable nếu số lượng sách > 0
                if (bookSelected.Quantity > 0)
                {
                    bookSelected.IsAvailable = true;
                }

                // Lưu cập nhật sách vào cơ sở dữ liệu
                _bookServices.UpdateBook(bookSelected);

                // Hiển thị thông báo thành công
                MessageBox.Show("Yêu cầu trả sách đã được phê duyệt!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                SearchButton_Click(null, null);
            }

        }
    }
}
