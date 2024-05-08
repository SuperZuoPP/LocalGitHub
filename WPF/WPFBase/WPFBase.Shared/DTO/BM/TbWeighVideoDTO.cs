using System;
using System.Collections.Generic;
using System.Text;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Shared.DTO.BM
{
    public class TbWeighVideoDto : BaseDto
    {
        private string factory;
        private string model; 
        private string videoType;
        private string videoTypeNo;
        private string iP;
        private string iPHistory;
        private string port;
        private string userName;
        private string passWord;
        private string channelnub;
        private string storage; 
        private string position;
        private string status; 
        private string weighHouseCodes;  
         
        public string Factory
        {
            get { return factory; }
            set { SetProperty<string>(ref factory, value); }
        }
        public string Model
        {
            get { return model; }
            set { SetProperty<string>(ref model, value); }
        }
          
        public string VideoType
        {
            get { return videoType; }
            set { SetProperty<string>(ref videoType, value); }
        }
        public string VideoTypeNo
        {
            get { return videoTypeNo; }
            set { SetProperty<string>(ref videoTypeNo, value); }
        }

        public string IP
        {
            get { return iP; }
            set { SetProperty<string>(ref iP, value); }
        }

        public string IPHistory
        {
            get { return iPHistory; }
            set { SetProperty<string>(ref iPHistory, value); }
        }
        public string Port
        {
            get { return port; }
            set { SetProperty<string>(ref port, value); }
        }


        public string UserName
        {
            get { return userName; }
            set { SetProperty<string>(ref userName, value); }
        }

        public string PassWord
        {
            get { return passWord; }
            set { SetProperty<string>(ref passWord, value); }
        }

        public string Channelnub
        {
            get { return channelnub; }
            set { SetProperty<string>(ref channelnub, value); }
        }

        public string Storage
        {
            get { return storage; }
            set { SetProperty<string>(ref storage, value); }
        }

 
         
        public string Position
        {
            get { return position; }
            set { SetProperty<string>(ref position, value); }
        }


        public string Status
        {
            get { return status; }
            set { SetProperty<string>(ref status, value); }
        }

        public string WeighHouseCodes
        {
            get { return weighHouseCodes; }
            set { SetProperty<string>(ref weighHouseCodes, value); }
        }
         
    }
}
