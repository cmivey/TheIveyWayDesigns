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
    /// Interaction logic for EditInventory.xaml
    /// </summary>
    public partial class EditInventory : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public EditInventory(int inventoryId)
        {
            InitializeComponent();
            txtInventoryId.Text = inventoryId.ToString();

            InventoryModel model = dbConnect.GetInventory().Where(i => i.InventoryId == inventoryId).SingleOrDefault();

            txtDescription.Text = model.Description;
            txtSize.Text = model.Size;
            txtQuantity.Text = model.Quantity.ToString();
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
            this.Hide();
        }

        private void btnUpdInventory_Click(object sender, RoutedEventArgs e)
        {
            dbConnect.UpdateInventory(new InventoryModel()
            { InventoryId = Convert.ToInt32(txtInventoryId.Text),
              Description = txtDescription.Text,
              Size = txtSize.Text,
              Quantity = Convert.ToDouble(txtQuantity.Text)
            });

            MessageBox.Show("Inventory updated successfully1");
        }
    }
}
