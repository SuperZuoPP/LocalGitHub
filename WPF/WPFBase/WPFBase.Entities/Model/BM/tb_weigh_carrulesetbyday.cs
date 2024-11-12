using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Entities.Model.BM
{
    [Table("tb_weigh_carrulesetbyday")]
    public partial class tb_weigh_carrulesetbyday
    {
        public int ID { get; set; }
        public string TypeRule { get; set; }
        public string CreateDate { get; set; }
        public string CarNumber { get; set; }
        public double? Weight { get; set; }
        public string Unit { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string WeighHouseName { get; set; }
        public string WeighID { get; set; }
        public bool? Status { get; set; }
        public string WeighHouseCodes { get; set; }
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
    }
}
