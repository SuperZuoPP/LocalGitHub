﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WPFBase.Extensions;
using WPFBase.Models;

namespace WPFBase.ViewModels.SMViewModel
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel(IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBar();
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            NavigationParameters param = new NavigationParameters();
            if (obj.NameSpace == "UserView")
            {
                param.Add("Value", 0);
            }
            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.NameSpace,param);
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        private ObservableCollection<MenuBar> menuBars;
        private readonly IRegionManager regionManager;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }


        void CreateMenuBar()
        {
            //  MenuIcon =Icon
            // Title = MenuName
            // MenuCode = NameSpace 
            //1系统管理
            //var menus = AuthorityMenu.AuthorityMenus.Where(menu => menu.ParentId == "1").ToList().OrderBy(i => i.Id);
            //MenuBars = new ObservableCollection<MenuBar>(menus);
            MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { Icon = "MicrosoftXboxControllerMenu", Title = "菜单管理", NameSpace = "MenuListView" });
            MenuBars.Add(new MenuBar() { Icon = "Account", Title = "用户管理", NameSpace = "UserView" });
            MenuBars.Add(new MenuBar() { Icon = "AccountMultiple", Title = "用户组管理", NameSpace = "UserGroupView" });
            MenuBars.Add(new MenuBar() { Icon = "ShieldLock", Title = "权限管理", NameSpace = "AuthorityView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "关于更多", NameSpace = "AboutView" });
        }
    }
}
