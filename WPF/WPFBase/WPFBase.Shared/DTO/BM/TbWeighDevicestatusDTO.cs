using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighDevicestatusDTO : BaseDto
    { 
        private string slaveDeviceName;
        private string slaveDeviceNo; 
        private string slaveDeviceType; 
        private string status; 
        private string weighHouseCodes;  
         
        public string SlaveDeviceName
        {
            get { return slaveDeviceName; }
            set { SetProperty<string>(ref slaveDeviceName, value); }
        }
        public string SlaveDeviceNo
        {
            get { return slaveDeviceNo; }
            set { SetProperty<string>(ref slaveDeviceNo, value); }
        }
          
        public string SlaveDeviceType
        {
            get { return slaveDeviceType; }
            set { SetProperty<string>(ref slaveDeviceType, value); }
        }
       
        public string Status
        {
            get { return status; }
            set { SetProperty<string>(ref status, value); }
        }

        public string WeighHouseCodes
        {
            get { return weighHouseCodes; }
            set { SetProperty<string>(ref weighHouseCodes, value); }
        }
         
    }
}
