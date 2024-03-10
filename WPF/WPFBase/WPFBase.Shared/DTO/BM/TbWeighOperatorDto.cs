using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared.Extensions;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighOperatorDto : BaseDto
    {
        private string userCode;
        private string userNumber;
        private string userName;
        private string passWord;
        private bool status;


        public string UserCode
        {
            get { return userCode; }
            set { SetProperty<string>(ref userCode, value); }
        }
        public string UserNumber
        {
            get { return userNumber; }
            set { SetProperty<string>(ref userNumber, value); }
        }

        public string UserName
        {
            get { return userName; }
            set { SetProperty<string>(ref userName, value); }
        }


        public string PassWord
        {
            get { return passWord; }
            set { SetProperty<string>(ref passWord, value); }
        }

        public bool Status
        {
            get { return status; }
            set { SetProperty<bool>(ref status, value); }
        }


    }
}
