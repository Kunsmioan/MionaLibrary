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
        LoanHistoryServices _loanHistoryServices;
        LoanHistory? loanHistory;
        User? reader;

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
            _loanHistoryServices = new();
            loanHistory = new()
            {
                BookId = bookSelected.Id,
                UserId = reader.Id,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(7),
            };
            _loanHistoryServices.AddLoanHistory(loanHistory);

            if (bookSelected.Quantity > 0)
            {
                bookSelected.Quantity = bookSelected.Quantity - 1;


                if (bookSelected.Quantity == 0)
                {
                    bookSelected.IsAvailable = false;
                }
                _bookServices = new();
                _bookServices.UpdateBook(bookSelected);
            }
            else
            {
                // Handle the case where the quantity is already zero
                throw new InvalidOperationException("Cannot reduce the quantity of a book that is already at zero.");
            }
            // Thực hiện logic mượn sách (ví dụ: cập nhật cơ sở dữ liệu)
            MessageBox.Show($"You have successfully borrowed the book: {bookSelected.Title}");

            // Cập nhật trạng thái sách thành không có sẵn
            //bookSelected.IsAvailable = false;


            // Cập nhật trạng thái sách thành không có sẵn (chưa cần)
            //bookSelected.IsAvailable = false;

            // Cập nhật lại giao diện
            loadData();
        }
    }
}
