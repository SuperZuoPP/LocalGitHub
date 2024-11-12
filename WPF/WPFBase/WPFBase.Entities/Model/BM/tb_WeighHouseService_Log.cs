using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_WeighHouseService_Log")]
    public partial class tb_WeighHouseService_Log
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
