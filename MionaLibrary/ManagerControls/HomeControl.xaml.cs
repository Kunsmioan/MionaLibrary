using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Data.SqlClient;
using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;
using MionaLibrary.BookControls;

namespace MionaLibrary.ManagerControls
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        User? manager;
        BookServices? _bookServices;

        public HomeControl()
        {
            InitializeComponent();

        }
        public void SetUser(User user)
        {
            manager = user;
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            try
            {
                _bookServices = new();
                // Fetch all books from the database
                if (_bookServices == null)
                {
                    MessageBox.Show("BookServices is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Fetch all books from the database
                List<Book> books = _bookServices.GetAllBooks();

                // Bind the books to the DataGrid
                SearchResultsDataGrid.ItemsSource = books;
            }
            catch (SqlException ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show($"Failed to load books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the visibility of the placeholder text
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
            else
            {
                PlaceholderText.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy loại tìm kiếm từ ComboBox
            var selectedSearchType = (SearchTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Lấy từ khóa tìm kiếm từ TextBox
            var searchTerm = SearchTextBox.Text.Trim();

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(selectedSearchType) || string.IsNullOrWhiteSpace(searchTerm))
            {
                SearchResultsDataGrid.ItemsSource = _bookServices.GetAllBooks();
            }

            // Thực hiện tìm kiếm
            var filteredBooks = PerformSearch(selectedSearchType, searchTerm);

            // Hiển thị kết quả trong DataGrid
            SearchResultsDataGrid.ItemsSource = filteredBooks;
        }

        private List<Book> PerformSearch(string searchType, string searchTerm)
        {
            // Lọc dữ liệu dựa trên loại tìm kiếm
            return _bookServices.GetAllBooksByFilter(searchType, searchTerm);
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Thực hiện tìm kiếm
                SearchButton_Click(sender, e);
            }
        }

        private void TitleButton_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("Không tìm thấy cửa sổ ReaderWindow.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Không thể lấy thông tin sách từ nút.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the parent window hosting this UserControl
            Window parentWindow = Window.GetWindow(this);

            AddBookControl addBookControl = new AddBookControl();

            // Check if the parent window is of type ReaderWindow
            if (parentWindow is ManagerWindow managerWindow)
            {
                // Replace the current content with the UpdateProfileControl
                managerWindow.MainContent.Content = addBookControl; // Assuming MainContent is a ContentControl in ReaderWindow
            }
            else
            {
                MessageBox.Show("Unable to locate the parent window or container.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}