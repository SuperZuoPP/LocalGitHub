using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Entities.Model.BM
{
    [Table("tb_weigh_weightnote")]
    public partial class tb_weigh_qxfb
    { 
        public int ID { get; set; }
        public int Flag { get; set; }
        public string Orgcode { get; set; }
        public string Orgname { get; set; }
        public string DdBh { get; set; }
        public string Ddhh { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string CarNum { get; set; }
        public double? Sfsl { get; set; }
        public string KunShu { get; set; }
        public string PhDate { get; set; }
        public string Thdw { get; set; }
        public string WeighHouseName { get; set; } 
        public string WeighHouseCodes { get; set; }
        public int? OperateBit { get; set; }
        public int? UploadBit { get; set; }
        public DateTime? UploadTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Fhr { get; set; }
        public string Mdbh { get; set; }
        public string ThHeadeId { get; set; }
        public string Remark { get; set; }
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
