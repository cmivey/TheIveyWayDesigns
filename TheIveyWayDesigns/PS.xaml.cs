using Microsoft.Reporting.WinForms;
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
        public PS(int customerId)
        {
            InitializeComponent();
            this._reportViewer.Reset();
            this._reportViewer.LocalReport.ReportPath = (@"TheIveyWayDesigns\PackingSlip.rdlc");
            ReportDataSource rds = new ReportDataSource("PackingSlip", dbConnect.GetPackingListInfo(customerId));
            this._reportViewer.LocalReport.DataSources.Clear();
            this._reportViewer.LocalReport.DataSources.Add(rds);
            this._reportViewer.LocalReport.Refresh();
        }
    }
}
