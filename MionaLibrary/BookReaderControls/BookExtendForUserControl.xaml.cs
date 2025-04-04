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

namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for BookExtendForUserControl.xaml
    /// </summary>
    public partial class BookExtendForUserControl : UserControl
    {
        Book? bookSelected;
        private readonly BookServices _bookServices;
        private readonly RenewalServices _renewalServices;
        private readonly LoanServices _loanServices;    
        User? reader = new();
        Loan? loanSelected = new();

        public BookExtendForUserControl()
        {
            InitializeComponent();
            _bookServices = new BookServices();
            _renewalServices = new RenewalServices();
            _loanServices = new LoanServices();
        }

        public void SetBookSelected(Book book)
        {
            bookSelected = book;
            loadData();
        }

        public void SetUser(User user)
        {
            reader = user;
            loadData();
        }

        public void SetLoanSelected(Loan loan)
        {
            loanSelected = loan;
            loadData();
        }

        private void loadData()
        {
            if (bookSelected != null)
            {
                // Refresh book data to get latest info including Language and Genre
                bookSelected = _bookServices.GetBookById(bookSelected.Id);
                
                if (bookSelected == null)
                {
                    MessageBox.Show("Could not load book details. The book may have been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Gán dữ liệu từ cơ sở dữ liệu vào các TextBlock
                TitleTextBlock.Text = bookSelected.Title;
                AuthorTextBlock.Text = bookSelected.Author;
                LanguageTextBlock.Text = bookSelected.Language?.Name ?? "Unknown Language";
                GenreTextBlock.Text = bookSelected.Genre?.Name ?? "Unknown Genre";
                DescriptionTextBlock.Text = bookSelected.Description;
                PublishYearTextBlock.Text = bookSelected.PublishYear.ToString();
                ISBNTextBlock.Text = bookSelected.Isbn;
                PageTextBlock.Text = bookSelected.Page.ToString();

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    BookImage.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Absolute));
                }
            }
            if(bookSelected != null && reader != null)
            {
                // Get the loan for the selected book and reader
                if (loanSelected != null)
                {
                    Extend.Text = loanSelected.RenewalCount.ToString();
                    BorrowDate.Text = loanSelected.BorrowDate.ToString("d");
                    DueDate.Text = loanSelected.DueDate.ToString("d");  
                }
                
            }   
        }

        private void ExtendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                if (loanSelected.Status == "Overdue")
                {
                    MessageBox.Show("This loan cannot be renewed. It was being overdue", "Cannot Renew", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                // Làm mới giao diện
                loadData();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
