using FastReport;
using FastReport.Export.Dbf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPFBase.Shared.DTO.BM;
using WPFBase.ViewModels.BMViewModel;

namespace WPFBase.Views.BMView
{
    /// <summary>
    /// QueryDataLineView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryDataLineView : UserControl
    {
        private QueryDataLineViewModel viewModel=>this.DataContext as QueryDataLineViewModel;
        public QueryDataLineView()
        {
            InitializeComponent(); 
        }

        private async  void Button_Click(object sender, RoutedEventArgs e)
        {
            var reportFile = System.IO.Path.Join(Environment.CurrentDirectory, "按计划单号与日期明细.frx");//按计划单号与日期明细 Simple List
            var report = new Report();
            report.Load(reportFile);
            await viewModel.SearchAsync();
            var ds = ConvertToDataSet(viewModel.WeighDataListsDtos);
            // var ds = ConvertToDataSet(viewModel.DataInfos);
            report.RegisterData(ds, "NorthWind");
            // var ds1 = TestData();
            // report.RegisterData(ds1, "NorthWind");
            //report.PrepareAsync(previewControl);
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

        public DataSet ConvertToDataSet(ObservableCollection<TbWeighDatalineinfoDto> weighDataList)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable("WeighDataline");//WeighDataline

            // 添加列到DataTable，与TbWeighDatalineinfoDto的属性对应
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("PlanNumber", typeof(string));
            dataTable.Columns.Add("CarNumber", typeof(string));
            dataTable.Columns.Add("SupplierName", typeof(string));
            dataTable.Columns.Add("RecipientName", typeof(string));
            dataTable.Columns.Add("BatchNumber", typeof(string));
            dataTable.Columns.Add("GrossWeight", typeof(double));
            dataTable.Columns.Add("TareWeight", typeof(double));
            dataTable.Columns.Add("Suttle", typeof(double));
            dataTable.Columns.Add("GrossWeighTime", typeof(DateTime));
           
            // 遍历ObservableCollection，并将每个对象转换为DataRow
            foreach (TbWeighDatalineinfoDto item in weighDataList)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Id"] = item.Id;
                dataRow["PlanNumber"] = item.PlanNumber;
                dataRow["CarNumber"] = item.CarNumber;
                dataRow["SupplierName"] = item.SupplierName;
                dataRow["RecipientName"] = item.RecipientName;
                dataRow["BatchNumber"] = item.BatchNumber;
                dataRow["GrossWeight"] = item.GrossWeight;
                dataRow["TareWeight"] = item.TareWeight;
                dataRow["Suttle"] = item.Suttle;
                dataRow["GrossWeighTime"] = item.GrossWeighTime;
                // 设置其他列的值，如果有的话...

                dataTable.Rows.Add(dataRow);
            }

            // 将DataTable添加到DataSet
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
    }
}
 