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
    /// Interaction logic for AddVendor.xaml
    /// </summary>
    public partial class AddVendor : Window
    {
        DatabaseConnections dbConnectt = new DatabaseConnections();

        public AddVendor()
        {
            InitializeComponent();
        }

        private void btnVendors_Click(object sender, RoutedEventArgs e)
        {
            Vendors vendor = new Vendors();
            vendor.Show();
            this.Close();
        }

        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            VendorsModel model = new VendorsModel()
            {
                Address = txtAddress.Text,
                City = txtCity.Text,
                PhoneNumber = txtPhoneNumber.Text,
                State = txtState.Text,
                VendorName = txtName.Text,
                Website = txtWebsite.Text,
                ZipCode = txtZipCode.Text
            };

            dbConnectt.AddVendor(model);

            txtAddress.Text = "";
            txtCity.Text = "";
            txtPhoneNumber.Text = "";
            txtState.Text = "";
            txtName.Text = "";
            txtWebsite.Text = "";
            txtZipCode.Text = "";

            MessageBox.Show("Vendor added successfully");
        }
    }
}
