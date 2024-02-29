using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighSqlstatement
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string TableDescription { get; set; }
        public string SqlCode { get; set; }
        public string SqlStatement { get; set; }
        public string Description { get; set; }
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
    }
}
