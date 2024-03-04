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

namespace WPFBase.ViewModels.Dialogs
{
    public class UserCreateViewModel : BindableBase, IDialogHostAware
    {
        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;
        public UserCreateViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            UserDto = new ResgiterDto();
            this.loginService = loginService;
            this.aggregator = aggregator;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            
        }

        public event Action<IDialogResult> RequestClose;

       

        #region 属性
        public string DialogHostName { get; set; }
        public string Title { get; set; }

        private ResgiterDto userDto;

        public ResgiterDto UserDto
        {
            get { return userDto; }
            set { SetProperty<ResgiterDto>(ref userDto, value); }
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


        private string passWord="111";

        public string PassWord
        {
            get { return passWord; }
            set { SetProperty<string>(ref passWord, value); }
        }

        private string newPassWord = "111";

        public string NewPassWord
        {
            get { return newPassWord; }
            set { SetProperty<string>(ref newPassWord, value); }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { SetProperty<int>(ref status, value); }
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

        async void Resgiter()
        {
            var loginResult = await loginService.Resgiter(new TbWeighOperatorDto() { UserNumber = UserNumber,UserName = UserName,PassWord="111", Status = Status });

            if (loginResult != null && loginResult.Status) 
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
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
            Title = parameters.GetValue<string>("Title");
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
