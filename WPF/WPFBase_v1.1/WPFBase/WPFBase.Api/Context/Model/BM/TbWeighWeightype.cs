using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighWeightype
    {
        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string WeighHouseCodes { get; set; }
        public bool? Status { get; set; }
        public int? OperateBit { get; set; }
        public int? UploadBit { get; set; }
        public DateTime? UploadTime { get; set; }
        public string CreateUserCode { get; set; }
        public string CreateUserName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string LastModifyUserCode { get; set; }
        public string LastModifyUserName { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public string Remark { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
        public string Attribute6 { get; set; }
        public string Attribute7 { get; set; }
        public string Attribute8 { get; set; }
    }
}
