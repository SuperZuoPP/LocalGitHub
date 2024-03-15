using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighGroupauthorityDto:BaseDto
    {
        private string userGroupCode;
        private string authorityCode;
        private bool status;
        public string UserGroupCode
        {
            get { return userGroupCode; }
            set { SetProperty<string>(ref userGroupCode, value); }
        }

        public string AuthorityCode
        {
            get { return authorityCode; }
            set { SetProperty<string>(ref authorityCode, value); }
        }

        public bool Status
        {
            get { return status; }
            set { SetProperty<bool>(ref status, value); }
        }
    }
}
