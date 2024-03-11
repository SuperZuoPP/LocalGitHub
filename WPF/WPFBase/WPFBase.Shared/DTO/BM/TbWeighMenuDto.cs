using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighMenuDto : BaseDto
    {
        private string menuCode;
        private string menuNumber;
        private string menuName;
        private bool status;

        public string MenuCode
        {
            get { return menuCode; }
            set { SetProperty<string>(ref menuCode, value); }
        }

        public string MenuNumber
        {
            get { return menuNumber; }
            set { SetProperty<string>(ref menuNumber, value); }
        }

        public string MenuName
        {
            get { return menuName; }
            set { SetProperty<string>(ref menuName, value); }
        }

        public bool Status
        {
            get { return status; }
            set { SetProperty<bool>(ref status, value); }
        }
    }
}
