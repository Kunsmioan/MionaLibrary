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
using MionaLibrary.ManagerControls;
using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;

namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for BookDetailsControl.xaml
    /// </summary>
    public partial class BookDetailsControl : UserControl
    {
        Book? bookSelected;
        BookServices? _bookServices;
        LoanHistoryServices _loanHistoryServices;
        LoanHistory loanHistory;
        User? reader;

        public BookDetailsControl()
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

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    BookImage.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Absolute));
                }

                if (bookSelected.IsAvailable)
                {
                    AvailableTextBlock.Text = "Yes";
                    AvailableTextBlock.Foreground = Brushes.Green; // Màu xanh lá
                    //BorrowBook.IsEnabled = true; // Cho phép nhấn nút BorrowBook
                }
                else
                {
                    AvailableTextBlock.Text = "No";
                    AvailableTextBlock.Foreground = Brushes.Red; // Màu đỏ
                    //BorrowBook.IsEnabled = false; // Vô hiệu hóa nút BorrowBook
                }
            }
        }


        private void UpdateBook_Click(object sender, RoutedEventArgs e)
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
                var updateBookControl = new UpdateBookControl();
                updateBookControl.SetBookSelected(bookSelected);

                //updateBookControl.SetUser(manager);

                // Thay thế nội dung hiện tại bằng BookDetailsControl
                mw.MainContent.Content = updateBookControl;
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
        $"Are you sure you want to delete the book '{bookSelected.Title}'?",
        "Confirm Deletion",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question);

            // Nếu người dùng chọn Yes, tiến hành xóa
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _bookServices = new BookServices();
                    _bookServices.DeleteBook(bookSelected);

                    // Cập nhật giao diện hoặc thông báo thành công
                    MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow is ManagerWindow mw)
                    {
                        User? manager = mw.GetManager();
                        //if (reader == null)
                        //{
                        //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //    return;
                        //}
                        var homeControl = new HomeControl();

                        //updateBookControl.SetUser(manager);

                        // Thay thế nội dung hiện tại bằng BookDetailsControl
                        mw.MainContent.Content = homeControl;
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    MessageBox.Show($"An error occurred while deleting the book:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
