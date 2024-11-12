using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_warnsearch")]
    public partial class tb_weigh_warnsearch
    {
        public int Id { get; set; }
        public string DataType { get; set; }
        public string CarNumber { get; set; }
        public string PlanCode { get; set; }
        public string PlanNumber { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public double? ExValue { get; set; }
        public string ExString { get; set; }
        public string WeighHouseCodes { get; set; }
        public DateTime? Createtime { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string QrCode { get; set; }
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
