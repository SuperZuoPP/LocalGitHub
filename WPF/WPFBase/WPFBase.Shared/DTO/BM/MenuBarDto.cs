using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.Extensions;

namespace WPFBase.Shared.DTO.BM
{
    public class MenuBarDto : BaseNotifyPropertyChanged
    {
        private int id;
        private string parentId;
        private string icon;
        private string title;
        private string nameSpace;

        public int Id
        {
            get { return id; }
            set { SetProperty<int>(ref id, value); }
        }

        public string ParentId
        {
            get { return parentId; }
            set { SetProperty<string>(ref parentId, value); }
        }

        public string Icon
        {
            get { return icon; }
            set { SetProperty<string>(ref icon, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty<string>(ref title, value); }
        }

        public string NameSpace
        {
            get { return nameSpace; }
            set { SetProperty<string>(ref nameSpace, value); }
        }
    }
}
