using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_deviceinfo")]
    public partial class tb_weigh_deviceinfo
    {
        public int Id { get; set; }
        public string DeviceCode { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceName { get; set; }
        public string WeighHouseCodes { get; set; }
        public string PortName { get; set; }
        public int? BaudRate { get; set; }
        public string Parity { get; set; }
        public int? DataBits { get; set; }
        public int? StopBits { get; set; }
        public bool? DtsEnable { get; set; }
        public bool? RtsEnable { get; set; }
        public int? ReceivedDataLength { get; set; }
        public bool? IsContinuousSend { get; set; }
        public string StartString { get; set; }
        public string EndString { get; set; }
        public bool? IsInvertedSequence { get; set; }
        public int? DataStaredIndex { get; set; }
        public int? DataLength { get; set; }
        public int? DataPricision { get; set; }
        public string ConvertType { get; set; }
        public string FilterString { get; set; }
        public bool? Status { get; set; }
        public bool? IsBasicRecord { get; set; }
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
