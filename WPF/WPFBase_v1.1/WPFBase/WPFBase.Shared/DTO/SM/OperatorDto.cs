using System;
using System.Collections.Generic;
using System.Text;

namespace WPFBase.Shared.DTO.SM
{
    public class OperatorDto : BaseDto
    {
        private string userCode;
        private string userNumber;
        private string userName;
        private string passWord;
        private int status;

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

        public int Status
        {
            get { return status; }
            set { SetProperty<int>(ref status, value); }
        }


    }
}
