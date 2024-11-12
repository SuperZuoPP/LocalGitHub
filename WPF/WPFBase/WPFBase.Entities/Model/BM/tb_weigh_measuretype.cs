using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_measuretype")]
    public partial class tb_weigh_measuretype
    {
        public int Id { get; set; }
        public string Carno { get; set; }
        public string Qrcode { get; set; }
        public string Ismeasure { get; set; }
        public string Isweight { get; set; }
        public string Weighhousecode { get; set; }
        public string Operabit { get; set; }
        public string Createdate { get; set; }
        public string Lastupdatetime { get; set; }
        public string Confirmflag { get; set; }
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
