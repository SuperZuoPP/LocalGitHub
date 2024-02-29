using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighDatalineinfo
    {
        public int Id { get; set; }
        public string WeighRecordNumber { get; set; }
        public string PlanCode { get; set; }
        public string PlanNumber { get; set; }
        public string BatchNumber { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string RecipientCode { get; set; }
        public string RecipientName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Specification { get; set; }
        public string Model { get; set; }
        public string CarNumber { get; set; }
        public string GrossWeighHouseCode { get; set; }
        public string GrossWeighMachineCode { get; set; }
        public double? GrossWeight { get; set; }
        public DateTime? GrossWeighTime { get; set; }
        public string GrossWeighManCode { get; set; }
        public string GrossWeighManName { get; set; }
        public string GrossWeighSupervisorCode { get; set; }
        public string GrossWeighSupervisor { get; set; }
        public string GrossWeighShift { get; set; }
        public string TareWeighHouseCode { get; set; }
        public string TareWeighMachineCode { get; set; }
        public double? TareWeight { get; set; }
        public DateTime? TareWeighTime { get; set; }
        public string TareWeighManCode { get; set; }
        public string TareWeighManName { get; set; }
        public string TareWeighSupervisorCode { get; set; }
        public string TareWeighSupervisor { get; set; }
        public string TareWeighShift { get; set; }
        public bool? IsManualInputTare { get; set; }
        public double? Suttle { get; set; }
        public string MeasureUnit { get; set; }
        public DateTime? WeighTime { get; set; }
        public double? Deduction { get; set; }
        public double? DeductionPercent { get; set; }
        public double? TwiceDeduction { get; set; }
        public double? TwiceDeductionPercent { get; set; }
        public double? MoistureContent { get; set; }
        public bool? IsManualInput { get; set; }
        public int? OperateBit { get; set; }
        public bool? IsSurplus { get; set; }
        public bool? IsReturnPurchase { get; set; }
        public string CreateUserCode { get; set; }
        public string CreateUserName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string LastModifiedUserCode { get; set; }
        public string LastModifiedUserName { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string Remark { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
        public string Attribute6 { get; set; }
        public string Attribute7 { get; set; }
        public string Attribute8 { get; set; }
        public string Attribute9 { get; set; }
        public string Attribute10 { get; set; }
        public string Attribute11 { get; set; }
        public string Attribute12 { get; set; }
        public string Attribute13 { get; set; }
        public string Attribute14 { get; set; }
        public string Attribute15 { get; set; }
    }
}
