using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MionaLibrary.UserControls;
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

namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for bookDetailsForUserControl.xaml
    /// </summary>
    public partial class bookDetailsForUserControl : UserControl
    {
        Book? bookSelected;
        BookServices? _bookServices;
        User? reader;
        LoanServices? _loanServices;

        BookRequestServices? _bookRequestServices = new();  

        public bookDetailsForUserControl()
        {
            InitializeComponent();
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

        private void loadData()
        {
            if (bookSelected != null)
            {
                // Gán dữ liệu từ cơ sở dữ liệu vào các TextBlock
                TitleTextBlock.Text = bookSelected.Title;
                AuthorTextBlock.Text = bookSelected.Author;
                LanguageTextBlock.Text = bookSelected.Language;
                GenreTextBlock.Text = bookSelected.Genre.Name;
                DescriptionTextBlock.Text = bookSelected.Description;
                PublishYearTextBlock.Text = bookSelected.PublishYear.ToString();
                ISBNTextBlock.Text = bookSelected.Isbn;
                PageTextBlock.Text = bookSelected.Page.ToString();
                StockTextBlock.Text = bookSelected.Quantity.ToString();

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    BookImage.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Absolute));
                }

                if (bookSelected.IsAvailable)
                {
                    AvailableTextBlock.Text = "Yes";
                    AvailableTextBlock.Foreground = Brushes.Green; // Màu xanh lá
                    BorrowBook.IsEnabled = true; // Cho phép nhấn nút BorrowBook
                }
                else
                {
                    AvailableTextBlock.Text = "No";
                    AvailableTextBlock.Foreground = Brushes.Red; // Màu đỏ
                    BorrowBook.IsEnabled = false; // Vô hiệu hóa nút BorrowBook
                }
            }
        }
        public void CreateBookRequest(int userId, int bookId)
        {
            var request = new BookRequest
            {
                UserId = userId,
                BookId = bookId,
                RequestDate = DateTime.Now,
                Status = "Pending" // Trạng thái ban đầu là "Pending"
            };

            _bookRequestServices.AddBookRequest(request);

        }

        private void BorrowBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra xem sách có sẵn để mượn hay không
                if (bookSelected.Quantity <= 0 || !bookSelected.IsAvailable)
                {
                    MessageBox.Show($"The book '{bookSelected.Title}' is currently unavailable for borrowing.");
                    return;
                }


                // Kiểm tra xem sách đã được mượn bởi người dùng hiện tại hay chưa
                _loanServices = new();
                bool isAlreadyBorrowed = _loanServices.IsBookBorrowedByUser(bookSelected.Id, reader.Id);
                if (isAlreadyBorrowed)
                {
                    MessageBox.Show($"You have already borrowed the book: {bookSelected.Title}", "Alert", MessageBoxButton.OK, MessageBoxImage.Stop);
                    // Vô hiệu hóa nút sau khi mượn sách
                    BorrowBook.IsEnabled = false;
                    return;
                }
                // Tạo yêu cầu mượn sách
                var bookRequestService = new BookRequestServices();
                bool hasPendingRequest = bookRequestService.HasPendingRequest(reader.Id, bookSelected.Id);
                if (hasPendingRequest)
                {
                    MessageBox.Show($"You have already requested to borrow the book: {bookSelected.Title}. Please wait for manager approval.", "Pending Request", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Vô hiệu hóa nút sau khi gửi yêu cầu
                    BorrowBook.IsEnabled = false;
                    return;
                }
                CreateBookRequest(reader.Id, bookSelected.Id);

                // Thông báo thành công
                MessageBox.Show($"Your request to borrow the book '{bookSelected.Title}' has been sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                //else
                //{
                //    _loanServices = new();

                //    //datetime to check
                //    string dateString = "16-3-2025";
                //    string format = "d-M-yyyy";
                //    DateTime borrowDate = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

                //    // Lưu lịch sử mượn sách
                //    Loan loan = new()
                //    {
                //        BookId = bookSelected.Id,
                //        UserId = reader.Id,
                //        //DueDate = borrowDate.AddDays(7),
                //        //BorrowDate = borrowDate,
                //        BorrowDate = DateTime.Now,
                //        DueDate = DateTime.Now.AddDays(7),
                //        ReturnDate = null,
                //        Status = "Borrowing"
                //    };
                //    _loanServices.AddLoan(loan);

                //    // Giảm số lượng sách đi 1
                //    bookSelected.Quantity -= 1;

                //    // Cập nhật trạng thái IsAvailable nếu số lượng sách bằng 0
                //    if (bookSelected.Quantity == 0)
                //    {
                //        bookSelected.IsAvailable = false;
                //    }

                //    // Lưu cập nhật sách vào cơ sở dữ liệu
                //    _bookServices = new();
                //    _bookServices.UpdateBook(bookSelected);

                //    // Hiển thị thông báo thành công
                //    MessageBox.Show($"You have successfully borrowed the book: {bookSelected.Title}");

                //    // Cập nhật lại giao diện
                //    loadData();

                //    // Vô hiệu hóa nút sau khi mượn sách
                BorrowBook.IsEnabled = false;
                //}


            }
            catch (SqlException ex)
            {
                // Xử lý lỗi nếu có vấn đề xảy ra
                MessageBox.Show($"An error occurred while borrowing the book: {ex.Message}");
            }
            loadData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is ReaderWindow rw)
            {
                User? manager = rw.GetReader();
                //if (reader == null)
                //{
                //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                var homeControl = new HomeControl();

                //updateBookControl.SetUser(manager);

                // Thay thế nội dung hiện tại bằng BookDetailsControl
                rw.MainContent.Content = homeControl;
            }
        }
    }
}
