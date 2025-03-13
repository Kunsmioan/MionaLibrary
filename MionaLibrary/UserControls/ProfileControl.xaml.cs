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

namespace MionaLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl
    {
        User? reader;
        public ProfileControl()
        {
            InitializeComponent();
        }

        private void LoadProfileData()
        {
            if (reader != null)
            {
                // Ensure that reader is not null before setting the content
                tbUsername.Text = reader.Username.ToString();
                tbfullName.Text = reader.FullName.ToString();
                if (reader.Birthday.HasValue)
                {
                    tbBirthday.Text = reader.Birthday.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    tbBirthday.Text = string.Empty; // Or any other default value if birthday is null
                }
                tbGender.Text = reader.Gender.ToString();
                tbPhone.Text = reader.Phone.ToString();
            }
            else
            {
                MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SetUser(User user)
        {
            reader = user;
            LoadProfileData();
            // You can refresh the UI or bind data here
        }
    }
}
