﻿using System;
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
using MionaLibrary_Services.Services;

namespace MionaLibrary.BookControls
{
    /// <summary>
    /// Interaction logic for BookDetailsControl.xaml
    /// </summary>
    public partial class BookDetailsControl : UserControl
    {
        Book? bookSelected;
        BookServices? _bookServices;
        LoanHistoryServices _loanHistoryServices;
        LoanHistory loanHistory;
        User? reader;
        public BookDetailsControl()
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

                // Load image path
                if (!string.IsNullOrEmpty(bookSelected.ImagePath))
                {
                    BookImage.Source = new BitmapImage(new Uri(bookSelected.ImagePath, UriKind.Relative));
                }
                else
                {
                    // Show a default image if ImagePath is empty
                    //BookImage.Source = new BitmapImage(new Uri("\BookImage\defaultBook.png", UriKind.Relative));
                }

                if (bookSelected.IsAvailable)
                {
                    AvailableTextBlock.Text = "Yes";
                    AvailableTextBlock.Foreground = Brushes.Green; // Màu xanh lá
                    //BorrowBook.IsEnabled = true; // Cho phép nhấn nút BorrowBook
                }
                else
                {
                    AvailableTextBlock.Text = "No";
                    AvailableTextBlock.Foreground = Brushes.Red; // Màu đỏ
                    //BorrowBook.IsEnabled = false; // Vô hiệu hóa nút BorrowBook
                }
            }
        }

        private void BorrowBook_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem sách có được chọn và có sẵn không
            if (bookSelected != null && bookSelected.IsAvailable)
            {
                // Hiển thị hộp thoại xác nhận
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to borrow the book: {bookSelected.Title}?",
                    "Confirm Borrow",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                // Nếu người dùng chọn "Yes"
                if (result == MessageBoxResult.Yes)
                {
                    if (reader != null)
                    {
                        loanHistory = new LoanHistory
                        {
                            BookId = bookSelected.Id,
                            UserId = reader.Id, // 
                            LoanDate = DateTime.Now,
                            DueDate = DateTime.Now.AddDays(7), // Số ngày mượn sách: 7 ngày
                            ReturnDate = DateTime.Now.AddDays(5),
                            OverdueDays = 0
                        };
                        _loanHistoryServices = new();
                        _loanHistoryServices.AddLoanHistory(loanHistory);

                        if (bookSelected.Quantity > 0)
                        {
                            bookSelected.Quantity = bookSelected.Quantity - 1;
                            

                            if (bookSelected.Quantity == 0)
                            {
                                bookSelected.IsAvailable = false;
                            }
                            _bookServices = new();
                            _bookServices.UpdateBook(bookSelected);
                        }
                        else
                        {
                            // Handle the case where the quantity is already zero
                            throw new InvalidOperationException("Cannot reduce the quantity of a book that is already at zero.");
                        }
                        // Thực hiện logic mượn sách (ví dụ: cập nhật cơ sở dữ liệu)
                        MessageBox.Show($"You have successfully borrowed the book: {bookSelected.Title}");

                        // Cập nhật trạng thái sách thành không có sẵn (chưa cần)
                        //bookSelected.IsAvailable = false;

                        // Cập nhật lại giao diện
                        loadData();
                    }
                }
            }
            else
            {
                // Nếu sách không có sẵn, hiển thị thông báo lỗi
                MessageBox.Show("This book is not available for borrowing.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
