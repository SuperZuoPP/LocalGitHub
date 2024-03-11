using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Extensions;
using WPFBase.Models;
using WPFBase.ViewModels.Dialogs;
using WPFBase.Common;
using Prism.Ioc;
using System.Windows.Data;
using System.ComponentModel;
using WPFBase.Services;
using WPFBase.Shared.DTO.BM;
using MaterialDesignThemes.Wpf;
using HandyControl.Controls;

namespace WPFBase.ViewModels.SMViewModel
{
    public class UserViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialog;
        private readonly ILoginService loginService;

        public UserViewModel(IContainerProvider provider, ILoginService loginService) : base(provider)
        {
            WeighOperatorDtos = new ObservableCollection<TbWeighOperatorDto>();
            this.regionManager = provider.Resolve<IRegionManager>();
            dialog = provider.Resolve<IDialogHostService>();
            this.loginService = loginService;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            EditCommand = new DelegateCommand<TbWeighOperatorDto>(Edit);
            DeleteCommand = new DelegateCommand<TbWeighOperatorDto>(Delete);
            PageUpdatedCommand = new DelegateCommand(PageUpdated);
            PerPageNumSeletedCommand = new DelegateCommand<string>(PerPageNumSeleted);
        }

        private void PerPageNumSeleted(string selectedItemContent)
        {
            Console.WriteLine("Selected Item Content: " + selectedItemContent);
        }




        #region 属性

        private int selectedIndex;

        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set 
            { 
                SetProperty<int>(ref selectedIndex, value);
                GetDataAsync();
            }
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

        private string searchText;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty<string>(ref searchText, value); }
        }

        private bool isRightDrawerOpen;

        /// <summary>
        /// 右侧编辑窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { SetProperty<bool>(ref isRightDrawerOpen, value); }
        }

        private int status = 1;

        /// <summary>
        /// 账户是否启用
        /// </summary>
        public int Status
        {
            get { return status; }
            set
            {
                SetProperty<int>(ref status, value);
                GetDataAsync();
            }
        }

        private int pageIndex = 1;

        public int PageIndex
        {
            get { return pageIndex; }
            set { SetProperty<int>(ref pageIndex, value); }
        }

        private int pageCount;

        public int PageCount
        {
            get { return pageCount; }
            set { SetProperty<int>(ref pageCount, value); }
        }

        private int perPageNum;

        public int PerPageNum
        {
            get { return perPageNum; }
            set { SetProperty<int>(ref perPageNum, value); }
        }

        private string test;

        public string Test
        {
            get { return test; }
            set { SetProperty<string>(ref test, value); }
        }

        private object selectedItem;

        public object SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty<object>(ref selectedItem, value); }
        }

        private string pageSum;

        public string PageSum
        {
            get { return pageSum; }
            set { SetProperty<string>(ref pageSum, value); }
        }

        private ObservableCollection<TbWeighOperatorDto> weighOperatorDtos;

        public ObservableCollection<TbWeighOperatorDto> WeighOperatorDtos
        {
            get { return weighOperatorDtos; }
            set { SetProperty<ObservableCollection<TbWeighOperatorDto>>(ref weighOperatorDtos, value); }
        }

        private TbWeighOperatorDto currentTbWeighOperatorDto;

        public TbWeighOperatorDto CurrentTbWeighOperatorDto
        {
            get { return currentTbWeighOperatorDto; }
            set { SetProperty<TbWeighOperatorDto>(ref currentTbWeighOperatorDto, value); }
        }
        #endregion

        #region 命令
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand<TbWeighOperatorDto> EditCommand { get; set; }

        public DelegateCommand<TbWeighOperatorDto> DeleteCommand { get; set; }

        public DelegateCommand PageUpdatedCommand { get; set; }

        public DelegateCommand<string> PerPageNumSeletedCommand { get; set; }
        

        #endregion

        #region 方法
        void Execute(string obj)
        {
            switch (obj)
            {
                case "Add": Add();  break;
                case "Search": GetDataAsync(); break;
                case "Resgiter": Resgiter(); break;
            }

        }

        async void GetDataAsync()
        {
            int? selectStatus;
            switch (SelectedIndex)
            {
                case 0:
                    selectStatus = null;
                    break;
                case 2:
                    selectStatus = 1;
                    break;
                default:
                    selectStatus = 0;
                    break;
            }

            var result = await loginService.GetAllFilterAsync(new Shared.Parameters.TbWeighOperatorDtoParameter()
            {
                PageIndex = PageIndex - 1,
                PageSize = PerPageNum,
                Search = SearchText,
                Status = selectStatus
            });

            if (result.Status)
            {
                WeighOperatorDtos.Clear();
                foreach (var item in result.Result.Items)
                {
                    WeighOperatorDtos.Add(item);
                }
            }
        }

        async void GetTotalSum()
        {
            try
            {
                var result = await loginService.Summary();
                if (result.Status)
                {
                   PageSum = $"共 {result.Result.ToString()} 条"; 
                   PageCount = (int)Math.Ceiling(Convert.ToDouble(result.Result.ToString()) / PerPageNum); 
                }
            }
            catch
            {

            }
        }

        private void Add()
        {
            CurrentTbWeighOperatorDto = new TbWeighOperatorDto();
            PassWord = NewPassWord = null;
            CurrentTbWeighOperatorDto.Status = true;
            IsRightDrawerOpen = true;
        }

        private async void Resgiter()
        {
            if (string.IsNullOrWhiteSpace(CurrentTbWeighOperatorDto.UserNumber) ||
                string.IsNullOrWhiteSpace(CurrentTbWeighOperatorDto.UserName) ||
                string.IsNullOrWhiteSpace(PassWord))
            {
                Growl.WarningGlobal("请输入完整的注册信息！");
                //aggregator.SendMessage("请输入完整的注册信息！", "Main");
                return;
            }

            if (string.IsNullOrWhiteSpace(PassWord)  || PassWord != NewPassWord )
            {
                Growl.WarningGlobal("密码不一致,请重新输入！");
                //aggregator.SendMessage("密码不一致,请重新输入！", "Main");
                return;
            }
            try
            {
                if (CurrentTbWeighOperatorDto.Id>0)
                {
                    CurrentTbWeighOperatorDto.PassWord = passWord;
                    var updateResult = await loginService.UpdateAsync(CurrentTbWeighOperatorDto);
                    if (updateResult.Status)
                    {
                        var operatormodel = weighOperatorDtos.FirstOrDefault(t => t.Id == CurrentTbWeighOperatorDto.Id);
                        if (operatormodel != null)
                        {
                            operatormodel.UserNumber = CurrentTbWeighOperatorDto.UserNumber;
                            operatormodel.UserName = CurrentTbWeighOperatorDto.UserName;
                            operatormodel.PassWord = PassWord;
                            operatormodel.Status = CurrentTbWeighOperatorDto.Status;
                            //aggregator.SendMessage("修改成功", "Main");
                            Growl.SuccessGlobal("修改成功！");
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    CurrentTbWeighOperatorDto.PassWord = passWord;
                    var addResult = await loginService.AddAsync(CurrentTbWeighOperatorDto);
                    if (addResult.Status)
                    {
                        weighOperatorDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                        //aggregator.SendMessage("添加成功", "Main");
                        Growl.SuccessGlobal("添加成功！");
                    }
                }
                GetTotalSum();
            }
            catch
            {
                Growl.ErrorGlobal("添加失败！");
               // aggregator.SendMessage("添加失败", "Main");
            } 
        }

        private async void Delete(TbWeighOperatorDto obj)
        {
            try
            { 
                var dialogResult = await dialog.Question("温馨提示", $"确认删除用户:{obj.UserName} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                var deleteResult = await loginService.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = WeighOperatorDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model != null)
                        WeighOperatorDtos.Remove(model);
                }
            }
            catch
            {

            }
        }

        private async void Edit(TbWeighOperatorDto obj)
        {
            var result = await loginService.GetFirstOfDefaultAsync(obj.Id);
            if (result.Status)
            {
                CurrentTbWeighOperatorDto = result.Result;
                PassWord = NewPassWord = null;
                IsRightDrawerOpen = true;
            }
        }


        private void PageUpdated()
        {
            GetDataAsync();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (navigationContext.Parameters.ContainsKey("Value"))
                SelectedIndex = navigationContext.Parameters.GetValue<int>("Value");
            else
                SelectedIndex = 0;
            GetTotalSum();
            GetDataAsync();
        }
        #endregion




    }

}
