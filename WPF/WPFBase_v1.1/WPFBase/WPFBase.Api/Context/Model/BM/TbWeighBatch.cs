using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighBatch
    {
        public int Id { get; set; }
        public string BatchCode { get; set; }
        public string BatchNumber { get; set; }
        public string BatchName { get; set; }
        public string WeighHouseCodes { get; set; }
        public bool? Status { get; set; }
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
    }
}
