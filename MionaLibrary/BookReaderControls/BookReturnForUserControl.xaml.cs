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

namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for BookReturnForUserControl.xaml
    /// </summary>
    public partial class BookReturnForUserControl : UserControl
    {
        Book? bookSelected;
         BookServices _bookServices = new BookServices();
        User? reader;


        public BookReturnForUserControl()
        {
            InitializeComponent();
        }

        public void SetBookSelected(Book book)
        {
            bookSelected = book;
            loadData();
        }

        public void SetUser(User user)
        {
            reader = user;
            loadData();
        }

        public void SetLoanSelected(Loan loan)
        {
            loanSelected = loan;
            loadData();
        }

        private void loadData()
        {
            if (bookSelected != null)
            {
                // Refresh book data to get latest info including Language and Genre
                bookSelected = _bookServices.GetBookById(bookSelected.Id);
                
                if (bookSelected == null)
                {
                    MessageBox.Show("Could not load book details. The book may have been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Gán dữ liệu từ cơ sở dữ liệu vào các TextBlock
                TitleTextBlock.Text = bookSelected.Title;
                AuthorTextBlock.Text = bookSelected.Author;
                LanguageTextBlock.Text = bookSelected.Language?.Name ?? "Unknown Language";
                GenreTextBlock.Text = bookSelected.Genre?.Name ?? "Unknown Genre";
                DescriptionTextBlock.Text = bookSelected.Description;
                PublishYearTextBlock.Text = bookSelected.PublishYear.ToString();
                ISBNTextBlock.Text = bookSelected.Isbn;
                PageTextBlock.Text = bookSelected.Page.ToString();

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    BookImage.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Absolute));
                }
            }
        }
    }
}
