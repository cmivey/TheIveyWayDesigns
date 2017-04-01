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
    /// Interaction logic for Vendors.xaml
    /// </summary>
    public partial class Vendors : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public Vendors()
        {
            InitializeComponent();

            IEnumerable<VendorsModel> vendors = dbConnect.GetVendors();

            dgVendors.ItemsSource = vendors;
        }

        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Close();
        }

        private void btnVendorProducst_Click(object sender, RoutedEventArgs e)
        {
            VendorsModel vendorInfo = ((FrameworkElement)sender).DataContext as VendorsModel;

            IEnumerable<VendorProductsModel> vendorProducts = dbConnect.GetVendorProducts(vendorInfo.VendorId);

            dgVendorProducts.Visibility = Visibility.Visible;
            lblVendorProducts.Visibility = Visibility.Visible;
            btnAddProduct.Visibility = Visibility.Visible;
            lblVendorProducts.Content = "Vendor Products for Vendor: " + vendorInfo.VendorName;
            lblVendorID.Content = vendorInfo.VendorId.ToString();

            dgVendorProducts.ItemsSource = vendorProducts;
        }

        private void btnEditVendor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddVendorProduct addProduct = new AddVendorProduct(Convert.ToInt32(lblVendorID.Content));
            addProduct.Show();
            this.Close();

        }
    }
}
