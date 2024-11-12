using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_charts_weigh")] 
    public partial class tb_weigh_charts_weigh
    {
        public int? Id { get; set; }
        public string CarNumber { get; set; }
        public double? TareWeight { get; set; }
        public DateTime? TareWeighTime { get; set; }
        public double? GrossWeight { get; set; }
        public DateTime? GrossWeighTime { get; set; }
        public double? Suttle { get; set; }
        public DateTime? WeighTime { get; set; }
    }
}
