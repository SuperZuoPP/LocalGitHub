using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighWeighbridgeofficeDTO : BaseDto
    {
        private string weighHouseCode;
        private string erpCode; 
        private string weighHouseName;
        private string companyName;
      
         
        public string WeighHouseCode
        {
            get { return weighHouseCode; }
            set { SetProperty<string>(ref weighHouseCode, value); }
        }
        public string ErpCode
        {
            get { return erpCode; }
            set { SetProperty<string>(ref erpCode, value); }
        }
          
        public string WeighHouseName
        {
            get { return weighHouseName; }
            set { SetProperty<string>(ref weighHouseName, value); }
        }
        public string CompanyName
        {
            get { return companyName; }
            set { SetProperty<string>(ref companyName, value); }
        }
 
    }
}
