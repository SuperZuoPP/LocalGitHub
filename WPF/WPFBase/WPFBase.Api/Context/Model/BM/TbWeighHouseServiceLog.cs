using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighHouseServiceLog
    {
        public string CommandId { get; set; }
        public string ReturnCommandId { get; set; }
        public string OpTable { get; set; }
        public string OpCommand { get; set; }
        public int? OpResult { get; set; }
        public DateTime? OpTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? OpType { get; set; }
        public string Beizhu { get; set; }
        public int? Records { get; set; }
    }
}
