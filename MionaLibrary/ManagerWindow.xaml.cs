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
using MionaLibrary.ManagerControls;
using MionaLibrary.BookControls;
using MionaLibrary.GenreControl;


namespace MionaLibrary
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public User? manager { get; set; }

        // Constructor
        public ManagerWindow()
        {
            InitializeComponent();
            MainContent.Content = new HomeControl();
        }

        // Phương thức để lấy Manager từ ManagerWindow
        public User? GetManager()
        {
            return manager;
        }

        // Method to load data and update UI elements
        private void loadData()
        {
            if (manager != null)
            {
                // Ensure that Manager is not null before setting the content
                ProfileButton.Content = manager.FirstName.ToString();

            }
            else
            {
                MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SetManager(User user)
        {
            manager = user;
            loadData();
        }

        // Method to handle the Profile button click
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            // Add the functionality for the Profile button click, if needed
            ProfileControl profileControl = new();
            MainContent.Content = profileControl;
            profileControl.SetUser(manager);
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            //if (manager == null)
            //{
            //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            // Tạo HomeControl và gán Manager
            HomeControl homeControl = new();
            homeControl.SetUser(manager);

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
            BookOnLoan placeOrder = new();
            MainContent.Content = placeOrder;
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
            NotificationManager notificationManager = new NotificationManager();
            MainContent.Content = notificationManager;
        }

        private void Genre_Click(object sender, RoutedEventArgs e)
        {
            GenreActionControl genreControl = new();
            MainContent.Content = genreControl;
        }
    }
}
