using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Extensions;
using WPFBase.ViewModels.SMViewModel;

namespace WPFBase.ViewModels.BMViewModel
{
    public class HomeViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;

        public HomeViewModel(IContainerProvider containerProvider, IRegionManager regionManager) : base(containerProvider)
        {
            this.regionManager = regionManager;
            ComponentCommand = new DelegateCommand<object>(ComponentCmd); 
        }

        private void ComponentCmd(object obj)
        {
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("WeightSiteView");
        }


        public DelegateCommand<object> ComponentCommand { get; set; }

    }
}
