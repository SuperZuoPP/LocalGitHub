using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFBase.Common;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.ViewModels.SMViewModel
{
    public class UserGroupViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialog; 

        public UserGroupViewModel(IContainerProvider provider) : base(provider)
        {
            ListData = new ObservableCollection<string> { "超级管理员组", "管理员组", 
                "用户组", "班长组", "司磅员组", "取样员组" };
            this.regionManager = provider.Resolve<IRegionManager>();
            dialog = provider.Resolve<IDialogHostService>();
            ShowRoleCommand = new DelegateCommand(ShowRoleList);
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }



        #region 属性
        private bool isLeftDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsLeftDrawerOpen
        {
            get { return isLeftDrawerOpen; }
            set { SetProperty<bool>(ref isLeftDrawerOpen, value); }
        }

        private bool isRightDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { SetProperty<bool>(ref isRightDrawerOpen, value); }
        }
        
        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        private ObservableCollection<string> listData;

        public ObservableCollection<string> ListData
        {
            get { return listData; }
            set { SetProperty<ObservableCollection<string>>(ref listData, value); }
        }
        #endregion

        #region 命令
        public DelegateCommand ShowRoleCommand { get; set; }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #endregion

        #region 方法
        private void ShowRoleList()
        {
            Console.WriteLine($"Selected item: {SelectedItem}");
        }

        void Execute(string obj)
        {
            switch (obj)
            {
                case "AddGroup": AddGroup(); break;
                case "AddUser":  AddUser(); break;
                    
            }

        }

        private void AddUser()
        {
            IsRightDrawerOpen = true;
        }

        private void AddGroup()
        {
             IsLeftDrawerOpen = true;
        }
        #endregion
    }
}
