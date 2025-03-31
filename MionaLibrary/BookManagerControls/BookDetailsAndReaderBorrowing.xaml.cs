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

namespace MionaLibrary.BookManagerControls
{
    /// <summary>
    /// Interaction logic for BookDetailsAndReaderBorrowing.xaml
    /// </summary>
    public partial class BookDetailsAndReaderBorrowing : UserControl
    {
        Book? bookSelected = new();
        //Loan? loanSelected = new();
        BookServices? _bookServices = new();
        LoanServices? _loanServices = new();
        User? reader = new();

        public BookDetailsAndReaderBorrowing()
        {
            InitializeComponent();
        }

        //public void SetLoanSelected(Loan loan)
        //{
        //    loanSelected = loan;
        //}

        public void SetBookSelected(Book book)
        {
            bookSelected = book;
        }

        public void SetUser(User user)
        {
            reader = user;
            loadData(); // Chỉ gọi loadData() một lần sau khi đã set cả book và user
        }

        private void loadData()
        {
            if (bookSelected != null)
            {
                // Refresh book data from database to get latest status
                bookSelected = _bookServices?.GetBookById(bookSelected.Id);
                
                if (bookSelected == null)
                {
                    MessageBox.Show("Could not load book details. The book may have been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Gán dữ liệu từ cơ sở dữ liệu vào các TextBlock
                TitleTextBlock.Text = bookSelected.Title;
                AuthorTextBlock.Text = bookSelected.Author;
                LanguageTextBlock.Text = bookSelected.Language.Name;
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
                    //BorrowBook.IsEnabled = true; // Cho phép nhấn nút BorrowBook
                }
                else
                {
                    AvailableTextBlock.Text = "No";
                    AvailableTextBlock.Foreground = Brushes.Red; // Màu đỏ
                    //BorrowBook.IsEnabled = false; // Vô hiệu hóa nút BorrowBook
                }

                //load reader borrowing
                LoadBooksOnLoanData();

            }
        }

        private void LoadBooksOnLoanData()
        {
            var bookOnLoan_List = _loanServices.GetBooksOnLoanOrOverdueByBookId(bookSelected.Id);

            // Gán dữ liệu vào DataGrid
            BooksOnLoanDataGrid.ItemsSource = bookOnLoan_List;
        }


        
        private void ReaderDetailsAndBooksOnloan_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
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
                var viewBooksOnLoan = new ViewBooksOnLoan();

                //updateBookControl.SetUser(manager);

                // Thay thế nội dung hiện tại bằng BookDetailsControl
                mw.MainContent.Content = viewBooksOnLoan;
            }
        }

    }
}
