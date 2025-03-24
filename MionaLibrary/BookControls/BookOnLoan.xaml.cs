using Microsoft.Data.SqlClient;
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
    /// Interaction logic for BookOnLoan.xaml
    /// </summary>
    public partial class BookOnLoan : UserControl
    {
        LoanServices? _loanServices;
        User? reader;

        public BookOnLoan()
        {
            InitializeComponent();
            
        }

        public void SetUser(User user)
        {
            reader = user;LoadData();
        }

        public void LoadData()
        {
            try
            {
                _loanServices = new LoanServices();

                if (_loanServices == null)
                {
                    MessageBox.Show("BookServices is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (reader != null)
                {
                    List<Loan> loanList = _loanServices.GetLoansByUserId(reader.Id);
                    BookOnLoanData.ItemsSource = loanList;
                }
                else
                {
                    MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (SqlException ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show($"Failed to load books on loan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
