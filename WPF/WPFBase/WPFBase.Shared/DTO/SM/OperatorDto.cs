using System;
using System.Collections.Generic;
using System.Text;

namespace WPFBase.Shared.DTO.SM
{
    public class OperatorDto:BaseDto
    {
        private string userCode;

        public string UserCode
        {
            get { return userCode; }
            set { SetProperty<string>(ref userCode, value); }
        }

        private string userNumber;

        public string UserNumber
        {
            get { return userNumber; }
            set { SetProperty<string>(ref userNumber, value); }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { SetProperty<string>(ref userName, value); }
        }

        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { SetProperty<string>(ref passWord, value); }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { SetProperty<int>(ref status, value); }
        }


    }
}
