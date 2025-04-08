using MionaLibrary.BookControls;
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

namespace MionaLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for HomeStatisticForReader.xaml
    /// </summary>
    public partial class HomeStatisticForReader : UserControl
    {
        LoanServices loanServices = new LoanServices();
        BookServices bookServices = new BookServices();
        UserServices userServices = new UserServices();
        public HomeStatisticForReader()
        {
            InitializeComponent();
            loadData();
        }

        public void loadData()
        {
            loadDataReadersBooksAndBorrowing();
            loadDataNewBooks();
            loadDataTopReaders();
            loadDataTopBooks();
        }

        public void loadDataReadersBooksAndBorrowing()
        {
            // Lấy danh sách tất cả các sách từ dịch vụ
            var booksCount = bookServices.GetTotalBooks();
            // Đếm số lượng sách
            int totalBooks = booksCount;
            // Hiển thị số lượng sách lên TextBlock
            txtTotalBooks.Text = totalBooks.ToString();

            var booksTitleCount = bookServices.GetTotalTitleBooks();
            // Đếm số lượng đầu sách
            int totalTitleBooks = booksTitleCount;
            // Hiển thị số lượng đầu sách lên TextBlock
            txtTotalTitleBooks.Text = totalTitleBooks.ToString();

            var readers = userServices.GetAll();
            // Đếm số lượng độc giả
            int totalReaders = readers.Count;
            // Hiển thị số lượng độc giả lên TextBlock
            txtTotalReaders.Text = totalReaders.ToString();
        }

        public void loadDataTopReaders()
        {
            // Lấy danh sách tất cả các sách từ dịch vụ
            var readers = loanServices.GetTopReaders();
            // Đếm số lượng sách đang được mượn
            dgTopReaders.ItemsSource = readers;
        }

        public void loadDataNewBooks()
        {
            // Lấy danh sách tất cả các sách từ dịch vụ
            var books = bookServices.GetBookWithEarliestCreateDate();
            // Lấy danh sách các sách mới nhất
            // Hiển thị số lượng sách mới lên TextBlock
            dgBooks.ItemsSource = books;
        }

        public void loadDataTopBooks()
        {
            // Lấy danh sách tất cả các sách từ dịch vụ
            var books = bookServices.GetTopBooks();
            // Đếm số lượng sách đang được mượn
            dgTopBooks.ItemsSource = books;
        }

        private void BookDetailsAndReaderBorrowing_Click(object sender, RoutedEventArgs e)
        {
            // Lấy đối tượng sách từ DataContext của nút
            if ((sender as Button)?.DataContext is Book selectedBook)
            {
                // Lấy cửa sổ cha
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow is ReaderWindow rw)
                {
                    User? reader = rw.GetReader();
                    //if (reader == null)
                    //{
                    //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //    return;
                    //}
                    var bookDetailsControl = new bookDetailsForUserControl();
                    bookDetailsControl.SetBookSelected(selectedBook);

                    bookDetailsControl.SetUser(reader);


                    // Thay thế nội dung hiện tại bằng BookDetailsControl
                    rw.MainContent.Content = bookDetailsControl;
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
