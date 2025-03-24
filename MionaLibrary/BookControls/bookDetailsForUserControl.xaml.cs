using Microsoft.Data.SqlClient;
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
    /// Interaction logic for bookDetailsForUserControl.xaml
    /// </summary>
    public partial class bookDetailsForUserControl : UserControl
    {
        Book? bookSelected;
        BookServices? _bookServices;
        LoanHistoryServices? _loanHistoryServices;
        LoanHistory? loanHistory;
        User? reader;
        LoanServices? _loanServices;

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
                GenreTextBlock.Text = bookSelected.Genre;
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
                else
                {
                    _loanServices = new();
                    // Lưu lịch sử mượn sách
                    Loan loan = new()
                    {
                        BookId = bookSelected.Id,
                        UserId = reader.Id,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(7),
                        ReturnDate = null,
                        Status = "Borrowing"
                    };
                    _loanServices.AddLoan(loan);

                    // Giảm số lượng sách đi 1

                    bookSelected.Quantity -= 1;

                    // Cập nhật trạng thái IsAvailable nếu số lượng sách bằng 0
                    if (bookSelected.Quantity == 0)
                    {
                        bookSelected.IsAvailable = false;
                    }

                    // Lưu cập nhật sách vào cơ sở dữ liệu
                    _bookServices = new();
                    _bookServices.UpdateBook(bookSelected);

                    // Hiển thị thông báo thành công
                    MessageBox.Show($"You have successfully borrowed the book: {bookSelected.Title}");

                    // Cập nhật lại giao diện
                    loadData();

                    // Vô hiệu hóa nút sau khi mượn sách
                    BorrowBook.IsEnabled = false;
                }


            }
            catch (SqlException ex)
            {
                // Xử lý lỗi nếu có vấn đề xảy ra
                MessageBox.Show($"An error occurred while borrowing the book: {ex.Message}");
            }
            loadData();
        }
    }
}
