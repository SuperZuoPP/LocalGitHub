using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Models
{
    public class Group : BindableBase
    {
        private string groupId;

        public string GroupId
        {
            get { return groupId; }
            set { SetProperty<string>(ref groupId, value); }
        }
        private string groupName;

        public string GroupName
        {
            get { return groupName; }
            set { SetProperty<string>(ref groupName, value); }
        }
         
    }
}
