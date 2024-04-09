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

            GoHomeCommand = new DelegateCommand(()=> 
            {
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("HomeView"); 
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

        public DelegateCommand GoHomeCommand { get; private set; }

        private ObservableCollection<MenuBar> menuBars;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string loginTime = "登录时间:["+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"]";

        public string LoginTime
        {
            get { return loginTime; }
            set { loginTime = value; RaisePropertyChanged(); }
        }
        

        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "HomeView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "树结构示例", NameSpace = "TreeDemoView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "报表管理", NameSpace = "QueryDataLineView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlusOutline", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统管理", NameSpace = "SettingsView" }); 
        }
       
        public void Configure()
        {
            UserName = "用户:[" + AppSession.UserName + "]";
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("HomeView");
        }
    }
}
