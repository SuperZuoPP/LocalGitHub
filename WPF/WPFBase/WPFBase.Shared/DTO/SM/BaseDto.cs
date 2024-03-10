using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.Extensions;

namespace WPFBase.Shared.DTO.SM
{
    public class BaseDto : BaseNotifyPropertyChanged
    {
        public int Id { get; set; }

        public int? OperateBit { get; set; }

        public string CreateUserCode { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateTime { get; set; }

        public string LastModifiedUserCode { get; set; }

        public string LastModifiedUserName { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public string Remark { get; set; }

        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; } 
        public string Attribute5 { get; set; }
        public string Attribute6 { get; set; }
        public string Attribute7 { get; set; }
        public string Attribute8 { get; set; }
        public string Attribute9 { get; set; }
        public string Attribute10 { get; set; }
        public string Attribute11 { get; set; }
        public string Attribute12 { get; set; }
        public string Attribute13 { get; set; }
        public string Attribute14 { get; set; }
        public string Attribute15 { get; set; }


    }
}