using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.Extensions;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighPlanDto : BaseNotifyPropertyChanged
    {
        private string planCode;


        public string PlanCode
        {
            get { return planCode; }
            set { SetProperty<string>(ref planCode, value); }
        }


    }
}