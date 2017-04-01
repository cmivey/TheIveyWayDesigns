using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public Customers()
        {
            InitializeComponent();

            
            IEnumerable<CustomerModel> customersModel = dbConnect.GetCustomers();

            dgCustomers.ItemsSource = customersModel;
        }

        private void btnVendors_Click(object sender, RoutedEventArgs e)
        {
            Vendors vendors = new Vendors();
            vendors.Show();
            this.Close();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            addCustomer.Show();
            this.Close();
        }

        private void dgCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int customerId = ((CustomerModel)dgCustomers.SelectedItem).CustomerId;
            EditCustomer editCustomer = new EditCustomer(customerId);
            editCustomer.Show();
            this.Close();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel customer = ((FrameworkElement)sender).DataContext as CustomerModel;

            AddOrder addOrder = new AddOrder(customer.CustomerId);
            addOrder.Show();
            this.Close();
        }

        private void ViewOrders_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel customer = ((FrameworkElement)sender).DataContext as CustomerModel;

            Orders orders = new Orders(customer.CustomerId);
            orders.Show();
            this.Close();
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            Orders orders = new Orders(null);
            orders.Show();
            this.Close();
        }
    }
}
