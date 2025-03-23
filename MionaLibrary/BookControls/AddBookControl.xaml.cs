using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AddBookControl.xaml
    /// </summary>
    public partial class AddBookControl : UserControl
    {
        BookServices? _bookServices;
        public AddBookControl()
        {
            InitializeComponent();
        }

        private void BtnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            // Tạo một OpenFileDialog để chọn tệp hình ảnh
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*",
                Title = "Select an Image"
            };

            // Hiển thị hộp thoại và kiểm tra nếu người dùng chọn tệp
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                // Kiểm tra xem đường dẫn có hợp lệ không
                if (File.Exists(imagePath))
                {
                    try
                    {
                        // Đặt hình ảnh được chọn làm nguồn của Image control
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Tải ngay lập tức
                        bitmap.EndInit();

                        imgBook.Source = bitmap;

                        // Lưu đường dẫn vào TextBox nếu cần
                        txtImagePath.Text = imagePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnAddBook_Click(object sender, RoutedEventArgs e)
        {

            if (!checkInput()) return;

            Book book = new()
            {
                Title = txtTitle.Text,
                Author = InputValidator.legitName(txtAuthor.Text),
                Genre = InputValidator.legitName(txtGenre.Text),
                Description = txtDescription.Text,
                ImagePath = txtImagePath.Text,
                Quantity = int.Parse(txtQuantity.Text),
                Language = InputValidator.legitName(txtLanguage.Text),
                Page = int.Parse(txtPage.Text),
                PublishYear = int.Parse(txtPublishYear.Text),
                Isbn = txtIsbn.Text,
            };

            // Try to register the user
            try
            {
                // Ensure _userServices is initialized before use
                _bookServices = new();

                // Call the register service to register the user
                _bookServices.AddBook(book);

                MessageBox.Show("Add book successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ManagerWindow parrentWindow = new();
                if (parrentWindow is ManagerWindow mw)
                {
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
            catch (InvalidOperationException ex)
            {
                // Log and display detailed error
                MessageBox.Show($"Add book failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool checkInput()
        {
            try
            {
                var messages = new List<(bool condition, string message)>
        {
            (!InputValidator.TextBoxesIsNotEmpty(txtTitle), "Please enter the book title!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtAuthor), "Please enter the author's name!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtPublishYear), "Please enter the publish year!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtIsbn), "Please enter the ISBN!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtQuantity), "Please enter the quantity!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtLanguage), "Please enter the language!"),
            (!InputValidator.validName(txtAuthor.Text), "Author's name contains invalid characters!"),
            (!InputValidator.validName(txtGenre.Text), "Genre contains invalid characters!"),
            (!InputValidator.IsNumeric(txtPublishYear.Text), "Publish year must be a valid number!"),
            (!InputValidator.IsNumeric(txtQuantity.Text), "Quantity must be a valid number!"),
            (!InputValidator.IsNumeric(txtPage.Text), "Page count must be a valid number!"),
            (!InputValidator.textBoxsLength(txtAuthor, txtIsbn, txtIsbn), "Word must be between 3 and 50 characters!"),
        };

                foreach (var (condition, message) in messages)
                {
                    if (condition)
                    {
                        MessageBox.Show(message, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"An error occurred during input validation:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
