using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.ViewModels.SMViewModel
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        public readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        //调用以确定此实例是否可以处理导航请求。
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        //当导航离开当前页时, 类似打开A, 再打开B时, 该方法被触发。
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        //导航完成前, 此处可以传递过来的参数以及是否允许导航等动作的控制。
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
         
    }
}
