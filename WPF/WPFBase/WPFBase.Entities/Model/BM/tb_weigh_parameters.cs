using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_parameters")]
    public partial class tb_weigh_parameters
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int? ColumnId { get; set; }
        public string ColumnName { get; set; }
        public string DefaultName { get; set; }
        public string AliasName { get; set; }
        public int? Length { get; set; }
        public bool? Status { get; set; }
        public string Type { get; set; }
        public bool? IsParameter { get; set; }
        public string SqlCodes { get; set; }
        public bool? IsResult { get; set; }
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
