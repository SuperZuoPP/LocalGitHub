using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_processstatus")]
    public partial class tb_weigh_processstatus
    {
        public int Id { get; set; }
        public string QrCode { get; set; }
        public int? Processstatus { get; set; }
        public int? Cargostatus { get; set; }
        public int? Deductionstatus { get; set; }
        public int? Isstrict { get; set; }
        public int? OperateBit { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
    }
}
