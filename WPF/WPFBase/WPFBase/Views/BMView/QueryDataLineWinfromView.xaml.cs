using FastReport; 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFBase.Shared.DTO.BM;
using WPFBase.ViewModels.BMViewModel;
using WPFBase.Views.Reports;

namespace WPFBase.Views.BMView
{
    /// <summary>
    /// QueryDataLineView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryDataLineWinfromView : UserControl
    {
        private QueryDataLineViewModel viewModel=>this.DataContext as QueryDataLineViewModel;
        //FastReport.Design.StandardDesigner.DesignerControl designer = new FastReport.Design.StandardDesigner.DesignerControl();
        //FastReport.Preview.PreviewControl prew = new FastReport.Preview.PreviewControl();
         


        public QueryDataLineWinfromView()
        {
            InitializeComponent(); 
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            //viewModel.Search(); 
            //var ds = ConvertToDataSet(viewModel.WeighDataListsDtos); 
            Preview prew = new Preview();
            prew.Show();
            //var ds = TestData();
            //FastReport.Report report = new FastReport.Report();
            //report.RegisterData(ds);
            //report.Show(); 
        }


        private DataSet TestData()
        {
            DataSet ds = new DataSet();

            // 创建一个DataTable
            DataTable table = new DataTable("tb_weigh_datalineinfo");

            // 定义列
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("PlanNumber", typeof(string));
            table.Columns.Add("CarNumber", typeof(string));

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
            // 添加其他列，如果有的话...

            // 遍历ObservableCollection，并将每个对象转换为DataRow
            foreach (TbWeighDatalineinfoDto item in weighDataList)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Id"] = item.Id;
                dataRow["PlanNumber"] = item.PlanNumber;
                dataRow["CarNumber"] = item.CarNumber;
                // 设置其他列的值，如果有的话...

                dataTable.Rows.Add(dataRow);
            }

            // 将DataTable添加到DataSet
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
    }
}
 