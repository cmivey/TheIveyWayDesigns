using System;
using System.Windows;
using TheIveyWayDesigns.Models;

namespace TheIveyWayDesigns
{
    /// <summary>
    /// Interaction logic for EditVendor.xaml
    /// </summary>
    public partial class EditVendor : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public EditVendor(int vendorId)
        {
            InitializeComponent();

            VendorsModel vendor = dbConnect.GetVendorById(vendorId);

            txtAddress.Text = vendor.Address;
            txtCity.Text = vendor.City;
            txtName.Text = vendor.VendorName;
            txtPhoneNumber.Text = vendor.PhoneNumber;
            txtState.Text = vendor.State;
            txtVendorId.Text = vendor.VendorId.ToString();
            txtWebsite.Text = vendor.Website;
            txtZipCode.Text = vendor.ZipCode;
        }

        private void btnVendors_Click(object sender, RoutedEventArgs e)
        {
            Vendors vendors = new Vendors();
            vendors.Show();
            this.Close();
        }

        private void btnUpdateVendor_Click(object sender, RoutedEventArgs e)
        {
            VendorsModel model = new VendorsModel()
            {
                Address = txtAddress.Text,
                City = txtCity.Text,
                PhoneNumber = txtPhoneNumber.Text,
                State = txtState.Text,
                VendorId =  Convert.ToInt32(txtVendorId.Text),
                VendorName = txtName.Text,
                Website = txtWebsite.Text,
                ZipCode = txtZipCode.Text
            };

            dbConnect.UpdateVendor(model);

            MessageBox.Show("Vendor Updated Successfully");


        }
    }
}
