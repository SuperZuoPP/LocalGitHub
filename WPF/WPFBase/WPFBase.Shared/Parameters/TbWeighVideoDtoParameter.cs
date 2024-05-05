using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.Shared.Parameters
{
    public class TbWeighVideoDtoParameter : QueryParameter
    {
         
        public int VideoTypeNo { get; set; }

        public int Status { get; set; }

        public string WeighHouseCodes { get; set; }
          
        public string DeviceNo { get; set; }

        public string DvrDeviceNo { get; set; }
        
    }
}
