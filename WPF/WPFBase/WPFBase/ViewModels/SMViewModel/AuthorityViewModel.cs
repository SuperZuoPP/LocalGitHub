using Prism.Commands;
using Prism.Ioc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace WPFBase.ViewModels.SMViewModel
{
    public class AuthorityViewModel : NavigationViewModel
    {

        public AuthorityViewModel(IContainerProvider provider) : base(provider)
        {
            
        }
    }
}
