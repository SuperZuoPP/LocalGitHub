using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WPFBase.Entities.BM
{
    [Table("tb_weigh_littleplan")]
    public partial class tb_weigh_littleplan
    {
        public int IntId { get; set; }
        public string Flag { get; set; }
        public string DealFlag { get; set; }
        public string ApplDate { get; set; }
        public string PlanCode { get; set; }
        public string WeighHouseCodes { get; set; }

        [Key]
        public string QrCode { get; set; }
        public string HeadreMark { get; set; }
        public string Eid { get; set; }
        public string Seq { get; set; }
        public string CarNo { get; set; }
        public string DriverName { get; set; }
        public string DriverIdnumber { get; set; }
        public string LoadWeight { get; set; }
        public string IslongPplan { get; set; }
        public string ReturnFlag { get; set; }
        public string LineRemark { get; set; }
        
        [Column("contacts_name")]
        public string ContactsName { get; set; }

        [Column("phone_num")]
        public string PhoneNum { get; set; }

        [Column("is_confirm")]
        public string IsConfirm { get; set; }
        public string Source { get; set; }

        [Column("is_weight")]
        public string IsWeight { get; set; }
        [Column("create_date")] 
        public string CreateDate { get; set; }

        [Column("last_update_date")]
        public string LastUpdateDate { get; set; }
        public int? OperateBit { get; set; }
        public int? UploadBit { get; set; }
        public DateTime? UploadTime { get; set; }
        public string CreateUserCode { get; set; }
        public string CreateUserName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string LastModifiedUserCode { get; set; }
        public string LastModifiedUserName { get; set; }
        public DateTime? LastModifiedTime { get; set; }
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
        public string Field11 { get; set; }
        public string Field12 { get; set; }
        public string Field13 { get; set; }
        public string Field14 { get; set; }
        public string Field15 { get; set; }
    }
}
