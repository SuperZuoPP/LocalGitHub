using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class MemoDto : BaseDto
    {
        private string title;
        private string content;
        private int status;

        public int Status
        {
            get { return status; }
            set { SetProperty<int>(ref status, value); }
        }

        public string Content
        {
            get { return content; }
            set { SetProperty<string>(ref content, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty<string>(ref title, value); }
        }






    }
}