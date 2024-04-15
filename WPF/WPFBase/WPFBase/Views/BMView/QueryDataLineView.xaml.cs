using FastReport;
using FastReport.Export.Dbf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFBase.Views.BMView
{
    /// <summary>
    /// QueryDataLineView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryDataLineView : UserControl
    { 
        public QueryDataLineView()
        {
            InitializeComponent(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var reportFile = System.IO.Path.Join(Environment.CurrentDirectory, "Simple List.frx");
            var report = new Report();
            report.Load(reportFile);
            var ds = TestData();
            report.RegisterData(ds, "NorthWind");
            report.PrepareAsync(previewControl); 
        }

        private DataSet TestData()
        {
            DataSet ds = new DataSet();

            // 创建一个DataTable
            DataTable table = new DataTable("Employees");

            // 定义列
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("FirstName", typeof(string));

            // 添加测试数据
            table.Rows.Add(1, "Name1", "zuochao");
            table.Rows.Add(2, "Name2", "zc");
            table.Rows.Add(3, "Name3", "pp");

            // 将DataTable添加到DataSet中
            ds.Tables.Add(table);
            return ds;
        }
    }
}
