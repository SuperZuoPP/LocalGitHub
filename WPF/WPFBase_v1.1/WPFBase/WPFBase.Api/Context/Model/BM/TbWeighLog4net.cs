using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighLog4net
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
