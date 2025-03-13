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
        }

        // Method to load data and update UI elements
        private void loadData()
        {
            if (reader != null)
            {
                // Ensure that reader is not null before setting the content
                ProfileButton.Content = reader.Username.ToString();

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

        private void Books_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Favorites_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Press OK to exit!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                new Login().Show();  // Tạo đối tượng Login và mở
                this.Close();  // Đóng cửa sổ hiện tại
            }
        }
    }
}
