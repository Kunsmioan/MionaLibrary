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

namespace MionaLibrary.BookEntityControl
{
    /// <summary>
    /// Interaction logic for LanguageActionControl.xaml
    /// </summary>
    public partial class LanguageActionControl : UserControl
    {
        LanguageServices? _services = new();
        Language? selectedLanguage = new();

        public LanguageActionControl()
        {
            InitializeComponent(); LoadData();
        }
        public void LoadData()
        {
            dataGridLanguages.ItemsSource = _services.GetLanguageList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Language Language = new Language
            {
                Name = txtLanguageName.Text
            };
            _services.AddLanguage(Language);
            LoadData();
        }

        private void dataGridLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lấy thể loại được chọn
            selectedLanguage = dataGridLanguages.SelectedItem as Language;

            // Hiển thị tên thể loại trong TextBox
            txtLanguageName.Text = selectedLanguage?.Name ?? string.Empty;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLanguage != null && !string.IsNullOrWhiteSpace(txtLanguageName.Text))
            {
                // Cập nhật tên của thể loại được chọn
                selectedLanguage.Name = txtLanguageName.Text.Trim();

                // Cập nhật lại DataGrid
                _services.UpdateLanguage(selectedLanguage);

                // Xóa nội dung trong TextBox
                txtLanguageName.Clear();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a Language to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Nút Delete
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLanguage != null)
            {
                // Hiển thị hộp thoại xác nhận
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the Language '{selectedLanguage.Name}'?",
                    "Confirm Delete",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);

                // Kiểm tra kết quả từ hộp thoại
                if (result == MessageBoxResult.OK)
                {
                    // Xóa thể loại khỏi danh sách
                    _services.RemoveLanguage(selectedLanguage);

                    // Cập nhật lại DataGrid
                    LoadData();

                    // Xóa nội dung trong TextBox
                    txtLanguageName.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select a Language to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
