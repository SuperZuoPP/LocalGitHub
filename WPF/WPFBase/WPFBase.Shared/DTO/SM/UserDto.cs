using System;
using System.Collections.Generic;
using System.Text;

namespace WPFBase.Shared.DTO.SM
{
    public class UserDto : BaseDto
    {
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

        private string account;

        public string Account
        {
            get { return account; }
            set { SetProperty<string>(ref account, value); }
        }

    }
}

