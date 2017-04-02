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
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public AddOrder(int customerId)
        {
            InitializeComponent();

            txtCustomerId.Text = customerId.ToString();
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Close();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescription.Text == "" || txtPrice.Text == "" || txtQuantity.Text == "")
            {
                MessageBox.Show("Please enter all data!");
                return;
            }

            List<AddOrderModel> addOrderModel = new List<AddOrderModel>();

            if (dgOrders.Items.Count == 0)
            {
                addOrderModel.Add(new AddOrderModel()
                {
                    Description = txtDescription.Text,
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    Price = Convert.ToDouble(txtPrice.Text),
                    LineTotal = Convert.ToDouble(txtPrice.Text) * Convert.ToDouble(txtQuantity.Text)
                });

                dgOrders.ItemsSource = addOrderModel;
            }
            else
            {
                foreach (AddOrderModel dr in dgOrders.ItemsSource)
                {
                    addOrderModel.Add(new AddOrderModel()
                    {
                        Description = dr.Description,
                        Quantity = Convert.ToInt32(dr.Quantity),
                        Price = Convert.ToDouble(dr.Price),
                        LineTotal = Convert.ToDouble(dr.Price) * Convert.ToDouble(dr.Quantity)
                    });
                }

                addOrderModel.Add(new AddOrderModel()
                {
                    Description = txtDescription.Text,
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    Price = Convert.ToDouble(txtPrice.Text),
                    LineTotal = Convert.ToDouble(txtPrice.Text) * Convert.ToDouble(txtQuantity.Text)
                });

                dgOrders.ItemsSource = addOrderModel;
            }

            txtDescription.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "1";

            MessageBox.Show("Order added Succesfully.  If this is the only item, click the Submit Orders button.");
        }

        private void btnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            List<AddOrderModel> addOrderModel = (List<AddOrderModel>)dgOrders.ItemsSource;

            dbConnect.AddOrder(addOrderModel, Convert.ToInt32(txtCustomerId.Text));

            MessageBox.Show("Orders Submitted Successfully");
        }
    }
}
