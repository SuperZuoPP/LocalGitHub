using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    [Table("tb_weigh_video")]
    public partial class TbWeighVideo
    {
        public int Id { get; set; }
        public string Factory { get; set; }
        public string Model { get; set; }
        public string VideoType { get; set; }
        public string VideoTypeNo { get; set; }
        public string Ip { get; set; }
        public string Iphistory { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Channelnub { get; set; }
        public string Storage { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
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
        public string Attribute10 { get; set; }
        public string Attribute11 { get; set; }
        public string Attribute12 { get; set; }
        public string Attribute13 { get; set; }
        public string Attribute14 { get; set; }
        public string Attribute15 { get; set; }
    }
}
