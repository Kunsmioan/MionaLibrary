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

namespace MionaLibrary.GenreControl
{
    /// <summary>
    /// Interaction logic for GenreActionControl.xaml
    /// </summary>
    public partial class GenreActionControl : UserControl
    {
        GenreServices? _services = new();
        Genre? selectedGenre = new();

        public GenreActionControl()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            dataGridGenres.ItemsSource = _services.GetAll();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Genre genre = new Genre
            {
                Name = txtGenreName.Text
            };
            _services.AddGenre(genre);
            LoadData();
        }

        private void dataGridGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lấy thể loại được chọn
            selectedGenre = dataGridGenres.SelectedItem as Genre;

            // Hiển thị tên thể loại trong TextBox
            txtGenreName.Text = selectedGenre?.Name ?? string.Empty;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGenre != null && !string.IsNullOrWhiteSpace(txtGenreName.Text))
            {
                // Cập nhật tên của thể loại được chọn
                selectedGenre.Name = txtGenreName.Text.Trim();

                // Cập nhật lại DataGrid
                _services.UpdateGenre(selectedGenre);

                // Xóa nội dung trong TextBox
                txtGenreName.Clear();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a genre to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Nút Delete
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGenre != null)
            {
                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the genre '{selectedGenre.Name}'?",
                    "Confirm Delete",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);

                // Kiểm tra kết quả từ hộp thoại
                if (result == MessageBoxResult.OK)
                {
                    // Xóa thể loại khỏi danh sách
                    _services.RemoveGenre(selectedGenre);

                    // Cập nhật lại DataGrid
                    LoadData();

                    // Xóa nội dung trong TextBox
                    txtGenreName.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select a genre to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
