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
    /// Interaction logic for EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : Window
    {
        public EditCustomer(int customerId)
        {
            InitializeComponent();

            DatabaseConnections dbConnect = new DatabaseConnections();

            var customers = dbConnect.GetCustomers().Where(c => c.CustomerId == customerId).SingleOrDefault();

            txtAddress.Text = customers.Address;
            txtCity.Text = customers.City;
            txtName.Text = customers.Name;
            txtPhoneNumber.Text = customers.PhoneNumber;
            txtState.Text = customers.State;
            txtZipCode.Text = customers.ZipCode;
            txtCustomerId.Text = customers.CustomerId.ToString();
        }

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel customerModel = new CustomerModel
            {
                Address = txtAddress.Text,
                City = txtCity.Text,
                CustomerId = Convert.ToInt32(txtCustomerId.Text),
                Name = txtName.Text,
                PhoneNumber = txtPhoneNumber.Text,
                State = txtState.Text,
                ZipCode = txtZipCode.Text
            };

            DatabaseConnections dbConnect = new DatabaseConnections();

            dbConnect.UpdateCustomer(customerModel);

            MessageBox.Show("Customer Updated Successfully");
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Close();
        }
    }
}
