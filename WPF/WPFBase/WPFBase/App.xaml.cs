using Prism.DryIoc;
using DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFBase.Common;
using WPFBase.ViewModels.Dialogs;
using WPFBase.ViewModels.SMViewModel;
using WPFBase.Views; 
using WPFBase.Views.SMView;
using WPFBase.Views.Dialogs;

namespace WPFBase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogHostService, DialogHostService>();

            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<TestView>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<AuthorityView, AuthorityViewModel>();
            containerRegistry.RegisterForNavigation<UserGroupView, UserGroupViewModel>();
            containerRegistry.RegisterForNavigation<UserCreateView,UserCreateViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
        }
    }
}
