using HandyControl.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Base;
using WPFBase.Models;
using WPFBase.Services;
using WPFBase.Shared;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        
        private readonly ILoginService loginService;
        private readonly ITbWeighWeighbridgeofficeService officeService;



        public LoginViewModel(ILoginService loginService, ITbWeighWeighbridgeofficeService officeService)
        { 
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.loginService = loginService;
            this.officeService = officeService;
            //ToDoDto todo = new ToDoDto() { Title="test",Content= "test", Status=1 };
            //SqliteHelper.InsertData1<ToDoDto,ToDo>(todo);

        }
        

       

        #region 属性


        public string Title { get; set; } = "SuperZuo";
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { SetProperty<string>(ref userName, value); }
        }

        private string userNumber = "zuochao";

        public string UserNumber
        {
            get { return userNumber; }
            set { SetProperty<string>(ref userNumber, value); }
        }

        private string passWord="111";

        public string PassWord
        {
            get { return passWord; }
            set { SetProperty<string>(ref passWord, value); }
        }
        #endregion


        #region 命令
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        #endregion

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": Login(); break;
                    // case "LoginOut": LoginOut(); break;
                    // case "Resgiter": Resgiter(); break;
                    // case "ResgiterPage": SelectIndex = 1; break;
                    //case "Return": SelectIndex = 0; break;
            }
        }

        async void Login()
        {
            if (string.IsNullOrWhiteSpace(UserNumber) ||
               string.IsNullOrWhiteSpace(PassWord))
            {
                return;
            }

            var loginResult = await loginService.Login(new Shared.DTO.BM.TbWeighOperatorDto
            {
                UserNumber = UserNumber,
                PassWord = PassWord
            });

            if (loginResult != null && loginResult.Status)
            {
                AppSession.UserName = loginResult.Result.UserName;
                AppSession.UserCode = loginResult.Result.UserCode;
                GetPoundRoomGroupList();
                var menulist = await loginService.MenuAuthority(loginResult.Result.UserCode);
                if (menulist.Status)
                {
                   var resultjson = menulist.Result.ToString(); ;
                   var result = JsonConvert.DeserializeObject<ObservableCollection<MenuBar>>(resultjson);
                    AuthorityMenu.AuthorityMenus = result; 
                }
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }
            else
            {
                //登录失败提示...
                Growl.WarningGlobal(loginResult.Message); 
            }
        }

        private async void GetPoundRoomGroupList()
        {

            var grouplists = await officeService.GetList();

            if (grouplists.Status)
            {
                AppSession.PoundRoomGroupList.Clear();
                foreach (var item in grouplists.Result.Items)
                {
                    AppSession.PoundRoomGroupList.Add(new PoundRoomGroup()
                    {
                        GroupId = item.WeighHouseCode,
                        GroupName = item.WeighHouseName
                    });
                }
            }
        }


        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
