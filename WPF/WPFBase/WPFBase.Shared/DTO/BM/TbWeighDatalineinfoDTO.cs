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
        private string supplierName;
        private string recipientName;
        private string materialName;
        private string batchNumber;
        private string carNumber;
        private double grossWeight;
        private double tareWeight;
        private double suttle;
        private string measureUnit;
        private DateTime grossWeighTime;
        private DateTime tareWeighTime;
        private double deduction;
        private string grossWeighMachineCode;
        private string tareWeighMachineCode; 
        private string grossWeighHouseCode; 
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

        public string MaterialName
        {
            get { return materialName; }
            set { SetProperty<string>(ref materialName, value); }
        }

        public string BatchNumber
        {
            get { return batchNumber; }
            set { SetProperty<string>(ref batchNumber, value); }
        }
        public string CarNumber
        {
            get { return carNumber; }
            set { SetProperty<string>(ref carNumber, value); }
        }


        public double GrossWeight
        {
            get { return grossWeight; }
            set { SetProperty<double>(ref grossWeight, value); }
        }

        public double TareWeight
        {
            get { return tareWeight; }
            set { SetProperty<double>(ref tareWeight, value); }
        }

        public double Suttle
        {
            get { return suttle; }
            set { SetProperty<double>(ref suttle, value); }
        }

        public string MasureUnit
        {
            get { return measureUnit; }
            set { SetProperty<string>(ref measureUnit, value); }
        }

        public DateTime GrossWeighTime
        {
            get { return grossWeighTime; }
            set { SetProperty<DateTime>(ref grossWeighTime, value); }
        }

        public DateTime TareWeighTime
        {
            get { return tareWeighTime; }
            set { SetProperty<DateTime>(ref tareWeighTime, value); }
        }


        public double Deduction
        {
            get { return deduction; }
            set { SetProperty<double>(ref deduction, value); }
        }


        public string GrossWeighMachineCode
        {
            get { return grossWeighMachineCode; }
            set { SetProperty<string>(ref grossWeighMachineCode, value); }
        }

        public string TareWeighMachineCode
        {
            get { return tareWeighMachineCode; }
            set { SetProperty<string>(ref tareWeighMachineCode, value); }
        }

        public string GrossWeighHouseCode
        {
            get { return grossWeighHouseCode; }
            set { SetProperty<string>(ref grossWeighHouseCode, value); }
        }

        public DateTime WeighTime
        {
            get { return weighTime; }
            set { SetProperty<DateTime>(ref weighTime, value); }
        }
    }
}
