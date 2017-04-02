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
using TheIveyWayDesigns.Models;

namespace TheIveyWayDesigns
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Close();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtCity.Text == "" || txtZipCode.Text == "" || txtState.Text == "")
            {
                MessageBox.Show("You must enter all required fields!");
                return;
            }

            CustomerModel model = new CustomerModel()
            {
                Address = txtAddress.Text,
                City = txtCity.Text,
                Name = txtName.Text,
                PhoneNumber = txtPhoneNumber.Text,
                State = txtState.Text,
                ZipCode = txtZipCode.Text
            };

            DatabaseConnections dbConnect = new DatabaseConnections();

            dbConnect.AddCustomer(model);

            MessageBox.Show("Customer Added Successfully");

            txtName.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZipCode.Text = "";
            txtPhoneNumber.Text = "";
        }
    }
}
