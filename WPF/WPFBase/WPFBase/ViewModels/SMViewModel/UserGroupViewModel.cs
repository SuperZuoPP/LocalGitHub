using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Extensions;
using WPFBase.Models; 
using WPFBase.ViewModels.Dialogs;
using WPFBase.Common;
using Prism.Ioc;
using System.Windows.Data;
using System.ComponentModel;

namespace WPFBase.ViewModels.SMViewModel
{
    public class UserGroupViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialog;

        public UserGroupViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
        {
            DataGridList = new ObservableCollection<DataGridList>();
            this.regionManager = provider.Resolve<IRegionManager>();
            this.dialog = dialog; 
            ExecuteCommand = new DelegateCommand<string>(Execute);
            CreateMenuBar();
        }

        async void Execute(string obj)
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value", "Hello"); //发送数据至弹窗 
            var dialogResult = await dialog.ShowDialog(obj, keys);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    aggregator.SendMessage("添加成功!");
                }
                catch
                {

                }
            }
        }
       

       
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        private ObservableCollection<DataGridList> dataGridList;
       

        public ObservableCollection<DataGridList> DataGridList
        {
            get { return dataGridList; }
            set { dataGridList = value; RaisePropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty<string>(ref title, value); }
        }

        private ICollectionView dataGridCollectionView;
        public ICollectionView DataGridCollectionView
        {
            get { return dataGridCollectionView; }
            set { SetProperty<ICollectionView>(ref dataGridCollectionView, value); }
        }
        void CreateMenuBar()
        {
            for (int i = 0; i < 20; i++)
            {

                DataGridList.Add(new DataGridList() {ID=i,Name=i.ToString(),Sex=(i%2).ToString(), Remark=i.ToString() });
            }
            CollectionViewSource collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = DataGridList;

            // 设置分组属性
            collectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Sex"));

            // 设置 DataGridCollectionView
            DataGridCollectionView = collectionViewSource.View;
        }
         
    }

    public class DataGridList 
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public string Remark { get; set; }
    }
}
