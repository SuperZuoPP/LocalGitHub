using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_processinfo")]
    public partial class tb_weigh_processinfo
    {
        public int Id { get; set; }
        public string Carnum { get; set; }
        public string Qrcode { get; set; }
        public string Proctype { get; set; }
        public string Procresult { get; set; }
        public DateTime? Createtime { get; set; }
        public string Createuser { get; set; }
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
