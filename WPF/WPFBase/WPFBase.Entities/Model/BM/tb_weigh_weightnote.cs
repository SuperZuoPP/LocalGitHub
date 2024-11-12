using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_weightnote")]
    public partial class tb_weigh_weightnote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Frx { get; set; }
        public int? DefaultCopies { get; set; }
        public int? Delflag { get; set; }
        public string Reprintmark { get; set; }
        public string Remark { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
    }
}
