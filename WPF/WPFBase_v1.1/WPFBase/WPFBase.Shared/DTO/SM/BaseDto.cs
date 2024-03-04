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
    }
}