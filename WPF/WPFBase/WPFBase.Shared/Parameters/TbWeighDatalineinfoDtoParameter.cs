using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.Shared.Parameters
{
    public class TbWeighDatalineinfoDtoParameter : QueryParameter
    { 
        public string PlanCode { get; set; }

        public string PlanNumber { get; set; }

        public string GrossWeighHouseCode { get; set; }

        public string MaterialName { get; set; }
        public string SupplierName { get; set; }
        public string RecipientName { get; set; }
        public string CarNumber { get; set; }
        public DateTime WeighTime { get; set; }= DateTime.Now;


    }
}
