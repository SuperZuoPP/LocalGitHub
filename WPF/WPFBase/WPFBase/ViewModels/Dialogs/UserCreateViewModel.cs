using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Common;
using WPFBase.Services;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.ViewModels.Dialogs
{
    public class UserCreateViewModel : BindableBase, IDialogHostAware
    {
        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;
        public UserCreateViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            TbWeighOperatorDto = new TbWeighOperatorDto();
            this.loginService = loginService;
            this.aggregator = aggregator;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            
        }
         

        public event Action<IDialogResult> RequestClose;

       

        #region 属性
        public string DialogHostName { get; set; }
        public string Title { get; set; }

        private TbWeighOperatorDto tbWeighOperatorDto;

        public TbWeighOperatorDto TbWeighOperatorDto
        {
            get { return tbWeighOperatorDto; }
            set { SetProperty<TbWeighOperatorDto>(ref tbWeighOperatorDto, value); }
        }

        private string userNumber;

        public string UserNumber
        {
            get { return userNumber; }
            set { SetProperty<string>(ref userNumber, value); }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { SetProperty<string>(ref userName, value); }
        }


        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { SetProperty<string>(ref passWord, value); }
        }

        private string newPassWord;

        public string NewPassWord
        {
            get { return newPassWord; }
            set { SetProperty<string>(ref newPassWord, value); }
        }

        private bool status;

        public bool Status
        {
            get { return status; }
            set { SetProperty<bool>(ref status, value); }
        }
        #endregion

        #region 命令
        public DelegateCommand<string> ExecuteCommand { get; set; }
        public DelegateCommand SaveCommand { get ; set; }
        public DelegateCommand CancelCommand { get; set ; }
         
        #endregion


        #region 方法
        private void Execute(string obj)
        {
            switch (obj)
            {
                // case "Login": Login(); break;
                // case "LoginOut": LoginOut(); break;
                case "Resgiter": Resgiter(); break;
                case "Cancel": Cancel(); break;
                    // case "ResgiterPage": SelectIndex = 1; break;
                    //case "Return": SelectIndex = 0; break;
            }
        }

        private async void Resgiter()
        {
            if (string.IsNullOrWhiteSpace(UserNumber) ||
                string.IsNullOrWhiteSpace(UserName) ||
                string.IsNullOrWhiteSpace(PassWord) ||
                string.IsNullOrWhiteSpace(NewPassWord))
            {
                aggregator.SendMessage("请输入完整的注册信息！", "Login");
                return;
            }

            if (PassWord != NewPassWord)
            {
                aggregator.SendMessage("密码不一致,请重新输入！", "Login");
                return;
            }
            var loginResult = await loginService.Resgiter(new TbWeighOperatorDto() { UserNumber = UserNumber,UserName = UserName,PassWord= PassWord, Status = Status });

            if (loginResult != null && loginResult.Status) 
            {
                aggregator.SendMessage("注册成功", "Main");
                Cancel();
            } 
            else
            {
                //登录失败提示...
                aggregator.SendMessage(loginResult.Message, "Login");
            }

        }

        private void Save()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value", "Hello");//返回的结果，名字为Value 值为Hello
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, keys));
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No)); //取消返回NO告诉操作结束
        }

        //接收数据
        public void OnDialogOpened(IDialogParameters parameters)
        {
            TbWeighOperatorDto = parameters.GetValue<TbWeighOperatorDto>("Value");
            UserNumber = TbWeighOperatorDto.UserNumber;
            UserName = TbWeighOperatorDto.UserName;
            Status = TbWeighOperatorDto.Status;
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }
        #endregion


    }
}
