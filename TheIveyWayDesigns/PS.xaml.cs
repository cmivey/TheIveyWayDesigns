using System;
using System.Windows;

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
                this._reportViewer.LocalReport.ReportEmbeddedResource = "TheIveyWayDesigns.Reports.PackingSlip.rdlc";

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
