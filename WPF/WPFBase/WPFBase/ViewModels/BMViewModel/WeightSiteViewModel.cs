using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Extensions;
using WPFBase.ViewModels.SMViewModel;
using WPFBase.Views.SMView;

namespace WPFBase.ViewModels.BMViewModel
{
    public class WeightSiteViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        public WeightSiteViewModel(IContainerProvider containerProvider, IRegionManager regionManager) : base(containerProvider)
        {
            this.regionManager = regionManager;
            ViewSelectCmd = new DelegateCommand<string>(ViewSelect);
        }

        private void ViewSelect(string obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj))
                return;
             
            regionManager.Regions[PrismManager.WeighSiteViewRegionName].RequestNavigate(obj);
        }

        public DelegateCommand<string> ViewSelectCmd { get; set; }     
    }
}
