using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Models
{
    /// <summary>
    /// 系统导航菜单实体类
    /// </summary>
    public class MenuBar : BindableBase
    {
        private string icon;
        private string title;
        private string nameSpace;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
         
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
         
        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }
    }
}
