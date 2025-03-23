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
namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for UpdateBookControl.xaml
    /// </summary>
    public partial class UpdateBookControl : UserControl
    {
        BookServices? _bookServices;
        Book? bookSelected;

        public UpdateBookControl()
        {
            InitializeComponent();
        }

        public void SetBookSelected(Book book)
        {
            bookSelected = book;
            loadData();
        }

        private void loadData()
        {
            if (bookSelected != null)
            {
                // Gán dữ liệu từ cơ sở dữ liệu vào các TextBlock
                txtTitle.Text = bookSelected.Title;
                txtAuthor.Text = bookSelected.Author;
                txtLanguage.Text = bookSelected.Language;
                txtGenre.Text = bookSelected.Genre;
                txtDescription.Text = bookSelected.Description;
                txtPublishYear.Text = bookSelected.PublishYear.ToString();
                txtIsbn.Text = bookSelected.Isbn;
                txtPage.Text = bookSelected.Page.ToString();
                txtQuantity.Text = bookSelected.Quantity.ToString();
                txtImagePath.Text = bookSelected.ImagePath;

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    imgBook.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Absolute));
                }

            }
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

        private void BtnUpdateBook_Click(object sender, RoutedEventArgs e)
        {

            bookSelected.Title = txtTitle.Text;
            bookSelected.Author = InputValidator.legitName(txtAuthor.Text);
            bookSelected.Genre = InputValidator.legitName(txtGenre.Text);
            bookSelected.Description = txtDescription.Text;
            bookSelected.ImagePath = txtImagePath.Text;
            bookSelected.Quantity = int.Parse(txtQuantity.Text);
            bookSelected.Language = InputValidator.legitName(txtLanguage.Text);
            bookSelected.Page = int.Parse(txtPage.Text);
            bookSelected.PublishYear = int.Parse(txtPublishYear.Text);
            bookSelected.Isbn = txtIsbn.Text;

            // Try to register the user
            try
            {
                // Ensure _userServices is initialized before use
                _bookServices = new();

                // Call the register service to register the user
                _bookServices.UpdateBook(bookSelected);

                MessageBox.Show("Update book successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                // Log and display detailed error
                MessageBox.Show($"Update book failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
