using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.ViewModels.SMViewModel;

namespace WPFBase.ViewModels.BMViewModel
{
    public class HomeViewModel : NavigationViewModel
    {
        public HomeViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {

        }
    }
}
