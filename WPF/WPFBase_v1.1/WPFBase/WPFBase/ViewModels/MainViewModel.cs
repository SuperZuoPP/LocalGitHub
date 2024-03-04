using Prism.DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Common;
using WPFBase.Extensions;
using WPFBase.Models;

namespace WPFBase.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel(IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Nagvigate); 
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            });

            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            });
        }

        private void Nagvigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LoginOutCommand { get; private set; }

        private ObservableCollection<MenuBar> menuBars;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private string userName="Hello";

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }


        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "\xe608", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe64c", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe64c", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe64a", Title = "系统设置", NameSpace = "SettingsView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe64a", Title = "权限管理", NameSpace = "AuthorityView" });
            MenuBars.Add(new MenuBar() { Icon = "\xe64a", Title = "测试", NameSpace = "TestView" });
        }
       
        public void Configure()
        { 
            CreateMenuBar();
            //regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
