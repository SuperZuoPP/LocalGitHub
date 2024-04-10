using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighDatalineinfoDto : BaseDto
    {
        private string planCode;
        private string planNumber;
        private string grossWeighHouseCode;
        private string materialName;
        private string supplierName;
        private string recipientName;
        private string carNumber;
        private DateTime weighTime;
       
        public string PlanCode
        {
            get { return planCode; }
            set { SetProperty<string>(ref planCode, value); }
        }
        public string PlanNumber
        {
            get { return planNumber; }
            set { SetProperty<string>(ref planNumber, value); }
        }

        public string GrossWeighHouseCode
        {
            get { return grossWeighHouseCode; }
            set { SetProperty<string>(ref grossWeighHouseCode, value); }
        }

        public string MaterialName
        {
            get { return materialName; }
            set { SetProperty<string>(ref materialName, value); }
        }
        public string SupplierName
        {
            get { return supplierName; }
            set { SetProperty<string>(ref supplierName, value); }
        }
        public string RecipientName
        {
            get { return recipientName; }
            set { SetProperty<string>(ref recipientName, value); }
        }
        public string CarNumber
        {
            get { return carNumber; }
            set { SetProperty<string>(ref carNumber, value); }
        }

        public DateTime WeighTime
        {
            get { return weighTime; }
            set { SetProperty<DateTime>(ref weighTime, value); }
        }
     
    }
}
