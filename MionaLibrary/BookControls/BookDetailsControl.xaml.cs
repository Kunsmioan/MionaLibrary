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
using MionaLibrary_DAL.Entity;

namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for BookDetailsControl.xaml
    /// </summary>
    public partial class BookDetailsControl : UserControl
    {
        Book? bookSelected;
        public BookDetailsControl()
        {
            InitializeComponent();
        }

        private void loadData()
        {
            if (bookSelected != null)
            {
                // Gán dữ liệu từ cơ sở dữ liệu vào các TextBlock
                TitleTextBlock.Text = bookSelected.Title;
                AuthorTextBlock.Text = bookSelected.Author;
                LanguageTextBlock.Text = bookSelected.Language;
                GenreTextBlock.Text = bookSelected.Genre;
                DescriptionTextBlock.Text = bookSelected.Description;
                PublishYearTextBlock.Text = bookSelected.PublishYear.ToString();
                ISBNTextBlock.Text = bookSelected.Isbn;
                PageTextBlock.Text = bookSelected.Page.ToString();
            }
        }

        public void SetBookSelected(Book book)
        {
            bookSelected = book;
            loadData();
        }

        private void BorrowBook_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
