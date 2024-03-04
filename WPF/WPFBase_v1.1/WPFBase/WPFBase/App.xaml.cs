using Prism.DryIoc;
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
using WPFBase.Views.Dialogs;
using WPFBase.Views.SMView;

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
            containerRegistry.RegisterDialog<UserCreateView, UserCreateViewModel>();

            containerRegistry.RegisterForNavigation<TestView>();
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<AuthorityView, AuthorityViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<UserGroupView, UserGroupViewModel>();
        }
    }
}
