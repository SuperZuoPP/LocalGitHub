using FastReport;
using FastReport.Barcode;
using FastReport.DevComponents.DotNetBar;
using HandyControl.Controls;
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
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WPFBase.Views.Reports
{
    /// <summary>
    /// Preview.xaml 的交互逻辑
    /// </summary>
    public partial class Preview : System.Windows.Window
    { 
        public static string FilePath
        {
            get { return string.Format(@"{0}\按计划单号与日期明细.frx", System.Windows.Forms.Application.StartupPath); }
        }

        FastReport.Preview.PreviewControl prew = new FastReport.Preview.PreviewControl(); 
        
        public Preview()
        {
            InitializeComponent();
            Report report = new Report();
            report.Load(FilePath);
            report.Preview = prew;
            report.Prepare();
            report.ShowPrepared();
            WindowsFormsHost1.Child = prew;
        }
          
    }
}
