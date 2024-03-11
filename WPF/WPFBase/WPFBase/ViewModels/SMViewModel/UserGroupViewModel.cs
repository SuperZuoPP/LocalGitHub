using HandyControl.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFBase.Common;
using WPFBase.Services;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.ViewModels.SMViewModel
{
    public class UserGroupViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialog;
        private readonly IUserGroupService service;

        public UserGroupViewModel(IContainerProvider provider, IUserGroupService service) : base(provider)
        {
            //用户列表
            UserListDatas = new ObservableCollection<TbWeighOperatorDto>();
            //当前用户组所包含的用户列表
            UserListDatasByGroup = new ObservableCollection<TbWeighGroupauthorityuserDto>();
            //用户组列表
            GroupListDatas = new ObservableCollection<TbWeighUsergroupDto>();
            //当前新增用户组
            CurrentGroup = new TbWeighUsergroupDto();
            //当前前用户组所包含的用户
            CurrentUserGroup = new TbWeighGroupauthorityuserDto(); 
            this.service = service;
            this.regionManager = provider.Resolve<IRegionManager>();
            dialog = provider.Resolve<IDialogHostService>();
            ShowRoleCommand = new DelegateCommand(ShowRoleList);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            AddUserCommand = new DelegateCommand(AddUserToGroup);
            RemoveUserCommand = new DelegateCommand(RemoveUserFromGroup);
            PageUpdatedCommand = new DelegateCommand(PageUpdated);
        }

       

        #region 属性

        private int pageIndex = 1;

        public int PageIndex
        {
            get { return pageIndex; }
            set { SetProperty<int>(ref pageIndex, value); }
        }

        private int pageSum;

        public int PageSum
        {
            get { return pageSum; }
            set { SetProperty<int>(ref pageSum, value);}
        }

        private string searchText;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty<string>(ref searchText, value); }
        }


        private bool isLeftDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsLeftDrawerOpen
        {
            get { return isLeftDrawerOpen; }
            set { SetProperty<bool>(ref isLeftDrawerOpen, value); }
        }
         
        private TbWeighUsergroupDto selectedGroupItem;
        public TbWeighUsergroupDto SelectedGroupItem
        {
            get { return selectedGroupItem; }
            set { SetProperty(ref selectedGroupItem, value); }
        }

        private TbWeighOperatorDto selectedUserItem;
        public TbWeighOperatorDto SelectedUserItem
        {
            get { return selectedUserItem; }
            set { SetProperty(ref selectedUserItem, value); }
        }

        private TbWeighGroupauthorityuserDto selectedUserGroupItem;
        public TbWeighGroupauthorityuserDto SelectedUserGroupItem
        {
            get { return selectedUserGroupItem; }
            set { SetProperty(ref selectedUserGroupItem, value); }
        }

        private TbWeighGroupauthorityuserDto currentUserGroup;
        public TbWeighGroupauthorityuserDto CurrentUserGroup
        {
            get { return currentUserGroup; }
            set { SetProperty(ref currentUserGroup, value); }
        }
         
        private ObservableCollection<TbWeighOperatorDto> userListDatas;

        public ObservableCollection<TbWeighOperatorDto> UserListDatas
        {
            get { return userListDatas; }
            set { SetProperty<ObservableCollection<TbWeighOperatorDto>>(ref userListDatas, value); }
        }

        private ObservableCollection<TbWeighGroupauthorityuserDto> userListDatasByGroup;

        public ObservableCollection<TbWeighGroupauthorityuserDto> UserListDatasByGroup
        {
            get { return userListDatasByGroup; }
            set { SetProperty<ObservableCollection<TbWeighGroupauthorityuserDto>>(ref userListDatasByGroup, value); }
        }

        private ObservableCollection<TbWeighUsergroupDto> groupListDatas;

        public ObservableCollection<TbWeighUsergroupDto> GroupListDatas
        {
            get { return groupListDatas; }
            set { SetProperty<ObservableCollection<TbWeighUsergroupDto>>(ref groupListDatas, value); }
        }

        private TbWeighUsergroupDto currentGroup;

        public TbWeighUsergroupDto CurrentGroup
        {
            get { return currentGroup; }
            set { SetProperty<TbWeighUsergroupDto>(ref currentGroup, value); }
        }

        #endregion

        #region 命令
        public DelegateCommand ShowRoleCommand { get; set; }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand AddUserCommand { get; set; }

        public DelegateCommand RemoveUserCommand { get; set; }

        public DelegateCommand PageUpdatedCommand { get; set; }

        #endregion

        #region 方法
        private void PageUpdated()
        {
            GetUserData();
        }

        private async void ShowRoleList()
        { 
            try
            {
                if (selectedGroupItem == null)
                    return;

                var grouplists = await service.GetUserGroupAndUserList(new Shared.Parameters.QueryParameter()
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = selectedGroupItem.UserGroupCode
                });

                if (grouplists.Status)
                {
                    UserListDatasByGroup.Clear();
                    foreach (var item in grouplists.Result.Items)
                    {
                        UserListDatasByGroup.Add(item);
                    }
                } 
            }
            catch  
            { 

            }
           
        }

        void Execute(string obj)
        {
            switch (obj)
            {
                case "AddGroup": AddGroup(); break;
                case "Search": GetUserData();break;
                case "SaveGroup": SaveGroup(); break; 
            } 
        }

       

        private void AddGroup()
        {
            
            CurrentGroup.Status = true;
            IsLeftDrawerOpen = true;
        }

        private async void SaveGroup()
        {
            if (string.IsNullOrWhiteSpace(CurrentGroup.UserGroupName))
            {
                Growl.WarningGlobal("请输入用户组名称！");
                //aggregator.SendMessage("请输入用户组名称！", "Main");
                return;
            }
            try
            {
                var addResult = await service.AddAsync(CurrentGroup);
                if (addResult.Status)
                {
                    GroupListDatas.Add(addResult.Result);
                    IsLeftDrawerOpen = false;
                    Growl.SuccessGlobal("添加用户组成功！");
                    //aggregator.SendMessage("添加用户组成功", "Main");
                }
            }
            catch
            {
                Growl.ErrorGlobal("添加用户组失败！");
                //aggregator.SendMessage("添加用户组失败", "Main");
            }
        }

        private  async void GetUserData()
        {
            var result = await service.GetUserList(new Shared.Parameters.TbWeighOperatorDtoParameter()
            {
                PageIndex = PageIndex - 1,
                PageSize = 20,
                Search = SearchText,
                Status = 1
            });

            if (result.Status)
            {
                UserListDatas.Clear();
                foreach (var item in result.Result.Items)
                {
                    UserListDatas.Add(item);
                }
            }
        }

        

        async void GetTotalUserSum()
        {
            try
            {
                var result = await service.GetUserSum();
                if (result.Status)
                {
                    PageSum = (int)Math.Ceiling(Convert.ToDouble(result.Result.ToString()) / 10);
                }
            }
            catch
            {

            }
        }


        private async void GetGroupList() 
        {
            var grouplists = await service.GetAllAsync(new Shared.Parameters.QueryParameter() {
                PageIndex = 0,
                PageSize = 100,
                Search = "" 
            });

            if (grouplists.Status)
            {
                GroupListDatas.Clear();
                foreach (var item in grouplists.Result.Items)
                {
                    GroupListDatas.Add(item);
                }
            }
        }

        private async void RemoveUserFromGroup()
        {
            if (selectedGroupItem is null)
            {
                Growl.WarningGlobal("请选择用户组！");
                //aggregator.SendMessage("请选择用户组", "Main");
                return;
            }

            if (SelectedUserGroupItem is null)
            {
                Growl.WarningGlobal("请选择移除的用户！");
                //aggregator.SendMessage("请选择移除的用户", "Main");
                return;
            }
            
            CurrentUserGroup.UserGroupCode = SelectedUserGroupItem.UserGroupCode;
            CurrentUserGroup.UserCode = SelectedUserGroupItem.UserCode;

            var result = await service.GroupUserRemove(CurrentUserGroup);

            if (result.Status)
            {
                ShowRoleList();
                UserListDatas.Remove(SelectedUserItem);
                Growl.SuccessGlobal("用户已移除用户组！");
            }
        }

        private async void AddUserToGroup()
        {
            if (selectedGroupItem is null)
            {
                Growl.WarningGlobal("请选择用户组！");
               // aggregator.SendMessage("请选择用户组", "Main");
                return;
            }

            if (SelectedUserItem is null)
            {
                Growl.WarningGlobal("请选择用户！");
                //aggregator.SendMessage("请选择用户", "Main");
                return;
            }
            CurrentUserGroup.UserGroupCode = selectedGroupItem.UserGroupCode;
            CurrentUserGroup.UserCode = selectedUserItem.UserCode;
            CurrentUserGroup.Attribute1 = selectedGroupItem.UserGroupName;
            CurrentUserGroup.Attribute2 = selectedUserItem.UserName;

            var result = await service.GroupUserAdd(CurrentUserGroup);

            if (result.Status)
            {
                ShowRoleList();
                UserListDatas.Remove(SelectedUserItem);
                //aggregator.SendMessage("添加成功", "Main");
                Growl.SuccessGlobal("用户添加成功！");
            }
            else
            {
                Growl.WarningGlobal("该用户已在用户组中！");
                //aggregator.SendMessage("该用户已在用户组中", "Main");
            } 
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            UserListDatasByGroup.Clear();
            GetUserData();
            GetTotalUserSum();
            GetGroupList();
        }
        #endregion
    }
}
