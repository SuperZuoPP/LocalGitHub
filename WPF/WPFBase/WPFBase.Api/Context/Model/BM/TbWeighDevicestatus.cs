using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    [Table("tb_weigh_devicestatus")]
    public partial class TbWeighDevicestatus
    {
        public int Id { get; set; }
        public string WeighHouseCodes { get; set; }
        public string SlaveDeviceName { get; set; }
        public string SlaveDeviceNo { get; set; }
        public int? Status { get; set; }
        public int? SlaveDeviceType { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string OperateBit { get; set; }
        public string UploadBit { get; set; }
        public DateTime? UploadTime { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
    }
}
