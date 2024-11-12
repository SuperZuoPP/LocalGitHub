using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_log4net")]
    public partial class tb_weigh_log4net
    {
        public int Id { get; set; }
        public DateTime? LogDate { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Username { get; set; }
        public string Weighhousename { get; set; }
        public string Ip { get; set; }
    }
}
