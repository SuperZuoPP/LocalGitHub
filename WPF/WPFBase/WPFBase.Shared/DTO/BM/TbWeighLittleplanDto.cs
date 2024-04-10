using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared.Extensions;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighLittleplanDto : BaseNotifyPropertyChanged
    {
        private string qrCode;
       
       
        public string QrCode
        {
            get { return qrCode; }
            set { SetProperty<string>(ref qrCode, value); }
        }
        
     
    }
}
