using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Models
{
    public static class AppSession
    {
        public static string UserName { get; set; }
        public static string UserCode { get; set; }

        public static ObservableCollection<PoundRoomGroup> PoundRoomGroupList = new ObservableCollection<PoundRoomGroup>();
    }
}
