using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_task")]
    public partial class tb_weigh_task
    {
        public int Id { get; set; }
        public string WeighHouseCode { get; set; }
        public string WeighHouseName { get; set; }
        public string WeighHouseIp { get; set; }
        public string UserName { get; set; }
        public string UserIp { get; set; }
        public int? Type { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
    }
}
