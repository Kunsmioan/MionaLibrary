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
using System.Windows.Shapes;
using MionaLibrary_DAL.Entity;
using MionaLibrary.UserControls;
using MionaLibrary.BookControls;

namespace MionaLibrary
{
    /// <summary>
    /// Interaction logic for ReaderWindow.xaml
    /// </summary>
    public partial class ReaderWindow : Window
    {
        public User? reader { get; set; }

        // Constructor
        public ReaderWindow()
        {
            InitializeComponent();
            MainContent.Content = new HomeControl();
        }

        // Phương thức để lấy reader từ ReaderWindow
        public User? GetReader()
        {
            return reader;
        }

        // Method to load data and update UI elements
        private void loadData()
        {
            if (reader != null)
            {
                // Ensure that reader is not null before setting the content
                ProfileButton.Content = reader.FirstName.ToString();

            }
            else
            {
                MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SetReader(User user)
        {
            reader = user;
            loadData();
        }

        // Method to handle the Profile button click
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            // Add the functionality for the Profile button click, if needed
            ProfileControl profileControl = new();
            MainContent.Content = profileControl;
            profileControl.SetUser(reader);
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (reader == null)
            {
                MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tạo HomeControl và gán reader
            HomeControl homeControl = new();
            homeControl.SetUser(reader);

            // Thay thế nội dung chính bằng HomeControl
            MainContent.Content = homeControl;
        }

        private void Books_Click(object sender, RoutedEventArgs e)
        {
            var template = BookButton.Template;

            // Tìm các biểu tượng trong ControlTemplate
            var angleRightIcon = template.FindName("AngleRightIcon", BookButton) as FontAwesome.WPF.ImageAwesome;
            var angleDownIcon = template.FindName("AngleDownIcon", BookButton) as FontAwesome.WPF.ImageAwesome;

            // Kiểm tra trạng thái hiện tại của BookOptions
            if (BookOptions.Visibility == Visibility.Collapsed)
            {
                // Hiển thị BookOptions và chuyển sang biểu tượng AngleDown
                BookOptions.Visibility = Visibility.Visible;
                angleRightIcon.Visibility = Visibility.Collapsed;
                angleDownIcon.Visibility = Visibility.Visible;
            }
            else
            {
                // Ẩn BookOptions và chuyển về biểu tượng AngleRight
                BookOptions.Visibility = Visibility.Collapsed;
                angleRightIcon.Visibility = Visibility.Visible;
                angleDownIcon.Visibility = Visibility.Collapsed;
            }
        }

        private void BookOnLoan_Click(object sender, RoutedEventArgs e)
        {
            if (reader == null)
            {
                MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BookOnLoan bookOnLoan = new();
            bookOnLoan.SetUser(reader);
            MainContent.Content = bookOnLoan;
        }

        private void BookBorowingLoan_Click(object sender, RoutedEventArgs e)
        {
            BookBorrowingLoan borrowBook = new();
            MainContent.Content = borrowBook;
        }

        //private void HistoryLoan_Click(object sender, RoutedEventArgs e)
        //{
        //    HistoryLoan historyLoan = new();
        //    MainContent.Content = historyLoan;
        //}

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Press OK to exit!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                new Login().Show();  // Tạo đối tượng Login và mở
                this.Close();  // Đóng cửa sổ hiện tại
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpControl helpControl = new();
            MainContent.Content = helpControl;
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
