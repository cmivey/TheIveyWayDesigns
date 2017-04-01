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

namespace TheIveyWayDesigns
{
    /// <summary>
    /// Interaction logic for AddVendorProduct.xaml
    /// </summary>
    public partial class AddVendorProduct : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public AddVendorProduct(int vendorId)
        {
            InitializeComponent();
            txtVendorId.Text = vendorId.ToString();
        }

        private void btnVendors_Click(object sender, RoutedEventArgs e)
        {
            Vendors vendors = new Vendors();
            vendors.Show();
            this.Close();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            VendorProductsModel model = new VendorProductsModel()
            {
                VendorId = Convert.ToInt32(txtVendorId.Text),
                Description = txtDescription.Text,
                Price = Convert.ToDouble(txtPrice.Text)
            };

            dbConnect.AddVendorProduct(model);

            txtDescription.Text = "";
            txtPrice.Text = "";
            MessageBox.Show("Vendor Product Added Successfully");

        }
    }
}
