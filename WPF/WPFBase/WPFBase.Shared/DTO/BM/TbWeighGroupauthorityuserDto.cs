using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighGroupauthorityuserDto : BaseDto
    {
        private string userGroupCode;
        private string userCode;
          
        public string UserGroupCode
        {
            get { return userGroupCode; }
            set { SetProperty<string>(ref userGroupCode, value); }
        }

        public string UserCode
        {
            get { return userCode; }
            set { SetProperty<string>(ref userCode, value); }
        }
        
    }
}
