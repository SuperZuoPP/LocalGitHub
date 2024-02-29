using System;
using System.Collections.Generic;

#nullable disable

namespace WPFBase.Api.Context.Model.BM
{
    public partial class TbWeighChartsWeigh
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
