﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TheIveyWayDesigns.Models;

namespace TheIveyWayDesigns
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        CreateInvoice createInvoice = new CreateInvoice();
        public Orders(int? customerId)
        {
            InitializeComponent();
            txtCustomerId.Text = customerId.ToString();
            
            IEnumerable<OrdersModel> orders = null;

            if (customerId != null)
            {
                orders = dbConnect.GetOrdersForCustomer(Convert.ToInt32(customerId));
                btnAddOrder.Visibility = Visibility.Visible;
            }
            else
            {
                orders = dbConnect.GetAllCurrentOrders();
                btnAddOrder.Visibility = Visibility.Hidden;
            }

            if (orders.Count() > 0)
            {
                dgOrders.ItemsSource = orders;
                dgOrders.Visibility = Visibility.Visible;
            }
            else
            {
                dgOrders.Visibility = Visibility.Hidden;
                MessageBox.Show("There are no current orders for this customer");
            }
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Close();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddOrder addOrder = new AddOrder(Convert.ToInt32(txtCustomerId.Text));
            addOrder.Show();
            this.Close();
        }

        private void btnViewOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            OrdersModel orderInfo = ((FrameworkElement)sender).DataContext as OrdersModel;

            IEnumerable<OrderDetailsModel> orderDetails = dbConnect.GetOrderDetails(orderInfo.OrderId);

            dgOrderDetails.Visibility = Visibility.Visible;
            lblOrderDetails.Visibility = Visibility.Visible;
            lblOrderDetails.Content = "Order Details for Order Number: " + orderInfo.OrderId.ToString();
            btnCreateInvoice.Visibility = Visibility.Visible;
            lblOrderId.Content = orderInfo.OrderId.ToString();
            btnViewInvoice.Visibility = Visibility.Visible;

            dgOrderDetails.ItemsSource = orderDetails;
        }

        private void btnPrintPackingSlip_Click(object sender, RoutedEventArgs e)
        {
            OrdersModel orderInfo = ((FrameworkElement)sender).DataContext as OrdersModel;

            PS ps = new PS(orderInfo.OrderId, orderInfo.CustomerId);
            ps.Show();
            this.Close();
        }

        private void btnCreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            createInvoice.CreatePayPalInvoice(Convert.ToInt32(lblOrderId.Content));
        }

        private void btnViewInvoice_Click(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice(Convert.ToInt32(lblOrderId.Content), Convert.ToInt32(txtCustomerId.Text));
            invoice.Show();
            this.Close();
        }
    }
}
