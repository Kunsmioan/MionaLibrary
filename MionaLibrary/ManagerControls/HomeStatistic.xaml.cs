using LiveCharts.Wpf;
using LiveCharts;
using MionaLibrary.BookControls;
using MionaLibrary.BookManagerControls;
using MionaLibrary.GenreControl;
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
        GenreServices genreServices = new GenreServices();
        public HomeStatistic()
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
            loadDataBooksGenreOnLoan();
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

            var genres = genreServices.GetAll();
            int totalGenres = genres.Count;
            txtGenre.Text = totalGenres.ToString();
            // Đếm số lượng thể loại
        }

        public void loadDataBooksGenreOnLoan()
        {
            // Lấy danh sách tất cả các sách từ dịch vụ
            var genreBorrowCounts = loanServices.GetGenreBorrowCounts();

            SeriesCollection pieData = new SeriesCollection();

            foreach (var item in genreBorrowCounts)
            {
                pieData.Add(new PieSeries
                {
                    Title = item.GenreName,
                    Values = new ChartValues<int> { item.BorrowCount },
                    DataLabels = true // Hiển thị nhãn dữ liệu
                });
            }

            // Gán dữ liệu vào biểu đồ
            MyPieChart.Series = pieData;

            // Đặt chú thích (legend)
            MyPieChart.LegendLocation = LegendLocation.Right;
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
                    var bookDetailsControl = new BookDetailsControl();
                    bookDetailsControl.SetBookSelected(selectedBook);

                    bookDetailsControl.SetUser(reader);


                    // Thay thế nội dung hiện tại bằng BookDetailsControl
                    rw.MainContent.Content = bookDetailsControl;
                }
                else if (parentWindow is ManagerWindow mw)
                {
                    User? manager = mw.GetManager();
                    //if (reader == null)
                    //{
                    //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //    return;
                    //}
                    var bookDetailsControl = new BookDetailsControl();
                    bookDetailsControl.SetBookSelected(selectedBook);

                    bookDetailsControl.SetUser(manager);


                    // Thay thế nội dung hiện tại bằng BookDetailsControl
                    mw.MainContent.Content = bookDetailsControl;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cửa sổ Window.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ReadersMoreInfo_Click(object sender, RoutedEventArgs e)
        {
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow is ManagerWindow mw)
                {
                    
                   ReadersControl readersControl = new ReadersControl();
                   mw.MainContent.Content = readersControl;

                }
                else
                {
                    MessageBox.Show("Không tìm thấy cửa sổ Window.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
        }

        private void BooksMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is ManagerWindow mw)
            {

                HomeControl homeControl = new HomeControl();
                mw.MainContent.Content = homeControl;

            }
            else
            {
                MessageBox.Show("Không tìm thấy cửa sổ Window.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenresMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is ManagerWindow mw)
            {
                GenreActionControl genreControl = new GenreActionControl();
                mw.MainContent.Content = genreControl;
            }
            else
            {
                MessageBox.Show("Không tìm thấy cửa sổ Window.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
