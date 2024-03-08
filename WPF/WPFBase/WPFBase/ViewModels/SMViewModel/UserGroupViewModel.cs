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
            UserListDatas = new ObservableCollection<TbWeighOperatorDto>();
            UserListDatasByGroup = new ObservableCollection<TbWeighOperatorDto>();
            GroupListDatas = new ObservableCollection<TbWeighUsergroupDto>();
            CurrentGroup = new TbWeighUsergroupDto();
            ListData = new ObservableCollection<string> { "超级管理员组", "管理员组", 
                "用户组", "班长组", "司磅员组", "取样员组" };
            this.service = service;
            this.regionManager = provider.Resolve<IRegionManager>();
            dialog = provider.Resolve<IDialogHostService>();
            ShowRoleCommand = new DelegateCommand(ShowRoleList);
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }



        #region 属性

        private int pageIndex = 1;

        public int PageIndex
        {
            get { return pageIndex; }
            set { SetProperty<int>(ref pageIndex, value); GetUserData(); }
        }

        private int pageSum;

        public int PageSum
        {
            get { return pageSum; }
            set { SetProperty<int>(ref pageSum, value); GetUserData(); }
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
        private ObservableCollection<string> listData;

        public ObservableCollection<string> ListData
        {
            get { return listData; }
            set { SetProperty<ObservableCollection<string>>(ref listData, value); }
        }

        private ObservableCollection<TbWeighOperatorDto> userListDatas;

        public ObservableCollection<TbWeighOperatorDto> UserListDatas
        {
            get { return userListDatas; }
            set { SetProperty<ObservableCollection<TbWeighOperatorDto>>(ref userListDatas, value); }
        }

        private ObservableCollection<TbWeighOperatorDto> userListDatasByGroup;

        public ObservableCollection<TbWeighOperatorDto> UserListDatasByGroup
        {
            get { return userListDatasByGroup; }
            set { SetProperty<ObservableCollection<TbWeighOperatorDto>>(ref userListDatasByGroup, value); }
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

        #endregion

        #region 方法
        private void ShowRoleList()
        {
            Console.WriteLine(SelectedGroupItem.UserGroupName);
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
                aggregator.SendMessage("请输入用户组名称！", "Main");
                return;
            }
            try
            {
                var addResult = await service.AddAsync(CurrentGroup);
                if (addResult.Status)
                {
                    GroupListDatas.Add(addResult.Result);
                    IsLeftDrawerOpen = false;
                    aggregator.SendMessage("添加用户组成功", "Main");
                }
            }
            catch
            {
                aggregator.SendMessage("添加用户组失败", "Main");
            }
        }

        private  async void GetUserData()
        {
            var result = await service.GetUserList(new Shared.Parameters.TbWeighOperatorDtoParameter()
            {
                PageIndex = 0,
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
                    PageSum = (int)Math.Ceiling(Convert.ToDouble(result.Result.ToString()) / 20);
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
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetUserData();
            GetTotalUserSum();
            GetGroupList();
        }
        #endregion
    }
}
