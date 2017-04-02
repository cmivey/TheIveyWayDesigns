﻿using Microsoft.Reporting.WinForms;
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
    /// Interaction logic for PS.xaml
    /// </summary>
    public partial class PS : Window
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public PS(int orderId, int customerId)
        {
            InitializeComponent();
            txtCustomerId.Text = customerId.ToString();
            txtOrderId.Text = orderId.ToString();

            _reportViewer.Load += ReportViewer_Load;
        }

        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
                PackingSlip dataset = new PackingSlip();

                dataset.BeginInit();

                reportDataSource1.Name = "PackingSlip"; //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dbConnect.GetPackingListInfo(Convert.ToInt32(txtOrderId.Text));
                this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                this._reportViewer.LocalReport.ReportEmbeddedResource = "TheIveyWayDesigns.PackingSlip.rdlc";

                dataset.EndInit();

                _reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            Orders order = new Orders(Convert.ToInt32(txtCustomerId.Text));
            order.Show();
            this.Close();
        }

        private void btnShipOrder_Click(object sender, RoutedEventArgs e)
        {
            dbConnect.ShipOrder(Convert.ToInt32(txtOrderId.Text));

            MessageBox.Show("Order number: " + Convert.ToInt32(txtOrderId.Text) + " has been set to shipped.");
        }
    }
}
