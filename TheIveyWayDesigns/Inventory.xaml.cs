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
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public Inventory()
        {
            InitializeComponent();

            IEnumerable<InventoryModel> inventoryModel = dbConnect.GetInventory();

            dgInventory.ItemsSource = inventoryModel;
        }

        
        private void btnAddInventory_Click(object sender, RoutedEventArgs e)
        {
            AddInventory addInventory = new AddInventory();
            addInventory.Show();
            this.Close();
        }

        private void btnViewCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customers customer = new Customers();
            customer.Show();
            this.Hide();
        }

        private void dgInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int inventoryId = ((InventoryModel )dgInventory.SelectedItem).InventoryId;
            EditInventory editInventory = new EditInventory(inventoryId);
            editInventory.Show();
            this.Close();
        }
    }
}
