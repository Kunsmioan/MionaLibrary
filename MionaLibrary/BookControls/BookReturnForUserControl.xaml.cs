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
    /// Interaction logic for BookReturnForUserControl.xaml
    /// </summary>
    public partial class BookReturnForUserControl : UserControl
    {
        Book? bookSelected;
        BookServices? _bookServices;
        LoanServices? _loanServices;
        Loan? loanSelected;
        User? reader;

        BookReturnRequestServices? _bookReturnRequestServices;

        public BookReturnForUserControl()
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

        public void SetLoanSelected(Loan loan)
        {
            loanSelected = loan;
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
                if (bookSelected.Genre != null)
                {
                    GenreTextBlock.Text = bookSelected.Genre.Name;
                }
                //else
                //{
                //    GenreTextBlock.Text = "Unknown Genre"; // Default value if Genre is null
                //}
                DescriptionTextBlock.Text = bookSelected.Description;
                PublishYearTextBlock.Text = bookSelected.PublishYear.ToString();
                ISBNTextBlock.Text = bookSelected.Isbn;
                PageTextBlock.Text = bookSelected.Page.ToString();
                //StockTextBlock.Text = bookSelected.Quantity.ToString();

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    BookImage.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Absolute));
                }

                //if (bookSelected.IsAvailable)
                //{
                //    AvailableTextBlock.Text = "Yes";
                //    AvailableTextBlock.Foreground = Brushes.Green; // Màu xanh lá
                //    //BorrowBook.IsEnabled = true; // Cho phép nhấn nút BorrowBook
                //}
                //else
                //{
                //    AvailableTextBlock.Text = "No";
                //    AvailableTextBlock.Foreground = Brushes.Red; // Màu đỏ
                //    //BorrowBook.IsEnabled = false; // Vô hiệu hóa nút BorrowBook
                //}
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra xem loanSelected và bookSelected có tồn tại không
                if (loanSelected == null || bookSelected == null)
                {
                    MessageBox.Show("Không thể lấy thông tin khoản vay hoặc sách.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var returnRequest = new ReturnRequest
                {
                    LoanId = loanSelected.Id,       // ID của khoản vay
                    BookId = bookSelected.Id,       // ID của sách
                    UserId = reader.Id,
                    RequestDate = DateTime.Now,     // Ngày gửi yêu cầu
                    Status = "Pending"              // Trạng thái mặc định là "Pending"
                };

                // Lưu yêu cầu trả sách vào cơ sở dữ liệu
                _bookReturnRequestServices = new();
                _bookReturnRequestServices.AddBookReturn(returnRequest);

                // Thông báo thành công
                MessageBox.Show($"Request return book '{bookSelected.Title}' has sent successfully. Please wait to approval.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                // Làm mới dữ liệu
                loadData();

                //// Cập nhật thông tin trả sách
                //loanSelected.ReturnDate = DateTime.Now;
                //loanSelected.Status = "Returned";

                //// Lưu thay đổi vào cơ sở dữ liệu
                //_loanServices = new();
                //_loanServices.UpdateLoan(loanSelected);

                //// Cập nhật số lượng sách
                //bookSelected.Quantity+=1;
                //_bookServices = new();
                //_bookServices.UpdateBook(bookSelected);

                // Thông báo thành công
                //MessageBox.Show($"Bạn đã trả sách thành công: {bookSelected.Title}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Làm mới dữ liệu
                //loadData();

                // Quay về màn hình BookOnLoan
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow is ReaderWindow rw)
                {
                    // Đảm bảo người dùng đã được chọn
                    User? reader = rw.GetReader();
                    if (reader == null)
                    {
                        MessageBox.Show("Không có người dùng nào được chọn. Vui lòng chọn người dùng trước.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Chuyển về màn hình BookOnLoan
                    var bookOnLoan = new BookOnLoan();
                    bookOnLoan.SetUser(reader); // Gán người dùng cho BookOnLoan
                    rw.MainContent.Content = bookOnLoan;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cửa sổ ReaderWindow.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi trả sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
