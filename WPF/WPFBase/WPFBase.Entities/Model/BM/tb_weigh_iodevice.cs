using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_iodevice")]
    public partial class tb_weigh_iodevice
    {
        public int Id { get; set; }
        public string IoDeciveNo { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
        public string RegisterAddress { get; set; }
        public string SlaveDeviceName { get; set; }
        public string SlaveDeviceNo { get; set; }
        public string SlaveDeviceChannel { get; set; }
        public int? SlaveDeviceType { get; set; }
        public int? SlaveDeviceDidotype { get; set; }
        public string Instruction { get; set; }
        public int? Status { get; set; }
        public string WeighHouseCodes { get; set; }
        public string Com { get; set; }
        public string BaudRate { get; set; }
        public string Parity { get; set; }
        public string DataBits { get; set; }
        public string StopBits { get; set; }
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
    }
}
