using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighUsergroupDto:BaseDto
    {
        
        private string userGroupName;
        private string weighHouseCodes;
        private bool status;

        public string UserGroupName
        {
            get { return userGroupName; }
            set { SetProperty<string>(ref userGroupName, value); }
        } 

        public string WeighHouseCodes
        {
            get { return weighHouseCodes; }
            set { SetProperty<string>(ref weighHouseCodes, value); }
        }
         
        public bool Status
        {
            get { return status; }
            set { SetProperty<bool>(ref status, value); }
        }
    }
}
