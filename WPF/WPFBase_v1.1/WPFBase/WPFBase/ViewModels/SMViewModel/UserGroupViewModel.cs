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
using WPFBase.Views.Dialogs;

namespace WPFBase.ViewModels.SMViewModel
{
    public class UserGroupViewModel : BindableBase, IDialogService
    {
        public UserGroupViewModel(IRegionManager regionManager, IDialogService dialog)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            this.regionManager = regionManager;
            this.dialog = dialog;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            CreateMenuBar();
        }

        private void Execute(string obj)
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Title","测试弹窗");//发送数据至弹窗
            dialog.ShowDialog(obj, keys,callback => {
                if (callback.Result == ButtonResult.OK)
                {
                    Title = callback.Parameters.GetValue<string>("Value");
                }
            });
        }

       

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.NameSpace);
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        private ObservableCollection<MenuBar> menuBars;
        private readonly IRegionManager regionManager;
        private readonly IDialogService dialog;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty<string>(ref title, value); }
        }


        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "\xe62f", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe62f", Title = "用户管理", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "\xe62f", Title = "权限管理", NameSpace = "AuthorityView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe64a", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "\xe62f", Title = "关于更多", NameSpace = "AboutView" });
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            throw new NotImplementedException();
        }
    }
}
