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
using WPFBase.Services.ServiceBase;
using WPFBase.Services;
using WPFBase.Views.BMView;
using WPFBase.ViewModels;
using Prism.Services.Dialogs;
using WPFBase.ViewModels.BMViewModel;

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
            //var service = App.Current.MainWindow.DataContext as IConfigureService;
            //if (service != null)
            //    service.Configure();
            //base.OnInitialized();
            var dialog = Container.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                    service.Configure();
                base.OnInitialized();
            });
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册httpclient
            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            //containerRegistry.GetContainer().RegisterInstance(ConfigurationManager.AppSettings["Url"], serviceKey: "webUrl");
            containerRegistry.GetContainer().RegisterInstance(@"https://localhost:44390/", serviceKey: "webUrl");

            //注册服务  
            containerRegistry.Register<ILoginService, LoginService>(); 
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<IUserGroupService, UserGroupService>();
            containerRegistry.Register<IMenuService, MenuService>();
            containerRegistry.Register<IDataInfoService, DataInfoService>();
            containerRegistry.Register<ITbWeighWeighbridgeofficeService, TbWeighWeighbridgeofficeService>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<TreeDemoView,TreeDemoViewModel>();
            containerRegistry.RegisterForNavigation<HomeView,HomeViewModel>();
            containerRegistry.RegisterForNavigation<AboutView>(); 
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<AuthorityView, AuthorityViewModel>();
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>();
            containerRegistry.RegisterForNavigation<UserGroupView, UserGroupViewModel>();
            containerRegistry.RegisterForNavigation<UserCreateView,UserCreateViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<MenuListView, MenuListViewModel>();
            containerRegistry.RegisterForNavigation<WeightSiteView,WeightSiteViewModel>();

            containerRegistry.RegisterForNavigation<DataInfoView, DataInfoViewModel>();
            containerRegistry.RegisterForNavigation<QueryDataLineView>();
            


        }
    }
}
