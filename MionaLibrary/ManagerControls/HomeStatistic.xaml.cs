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

namespace MionaLibrary.ManagerControls
{
    /// <summary>
    /// Interaction logic for HomeStatistic.xaml
    /// </summary>
    public partial class HomeStatistic : UserControl
    {
        LoanServices loanServices = new LoanServices();
        BookServices bookServices = new BookServices();
        UserServices userServices = new UserServices();
        public HomeStatistic()
        {
            InitializeComponent();
            loadData();
        }

        public void loadData()
        {
           loadDataReadersBooksAndBorrowing();
            loadDataNewBooks();
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

        public void loadDataNewBooks()
        {
            // Lấy danh sách tất cả các sách từ dịch vụ
            var books = bookServices.GetBookWithEarliestCreateDate();
            // Lấy danh sách các sách mới nhất
            // Hiển thị số lượng sách mới lên TextBlock
            dgBooks.ItemsSource = books;
        }

        private void ReaderDetailsAndBooksOnloan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
