using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_plan")]
    public partial class tb_weigh_plan
    {
        public int Id { get; set; }
        [Key]
        public string PlanCode { get; set; }
        public string PlanNumber { get; set; }
        public string OrganizationCode { get; set; }
        public string WeighHouseCodes { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string RecipientCode { get; set; }
        public string RecipientName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string WeighType { get; set; }
        public string MeasureUnit { get; set; }
        public DateTime? PlanBeginTime { get; set; }
        public DateTime? PlanEndTime { get; set; }
        public DateTime? PlanExecuteTime { get; set; }
        public double? PlanAmount { get; set; }
        public double? PlanExecuteAmount { get; set; }
        public bool? IsTotalControl { get; set; }
        public double? TotalFloatRange { get; set; }
        public double? TotalFloatPerRange { get; set; }
        public bool? IsCarControl { get; set; }
        public bool? IsFromErp { get; set; }
        public bool? Status { get; set; }
        public bool? EnableModify { get; set; }
        public bool? EnableDelete { get; set; }
        public int? OperateBit { get; set; }
        public int? UploadBit { get; set; }
        public DateTime? UploadTime { get; set; }
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
        public int? PrintNum { get; set; }
        public double? GrossWeightWarn { get; set; }
        public double? TareWeightWarn { get; set; }
        public double? SuttleWeightWarn { get; set; }
        public bool? IsSampling { get; set; }
        public string OrderId { get; set; }
    }
}
