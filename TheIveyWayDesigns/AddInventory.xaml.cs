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
    /// Interaction logic for AddInventory.xaml
    /// </summary>
    public partial class AddInventory : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public AddInventory()
        {
            InitializeComponent();
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
            this.Close();
        }

        private void btnAddInventory_Click(object sender, RoutedEventArgs e)
        {
            dbConnect.AddInventory(new Models.InventoryModel() { Description = txtDescription.Text, Quantity = Convert.ToDouble(txtQuantity.Text), Size = txtSize.Text });

            MessageBox.Show("Inventory Added Successfully");
            txtDescription.Text = "";
            txtSize.Text = "";
            txtQuantity.Text = "";
        }
    }
}
