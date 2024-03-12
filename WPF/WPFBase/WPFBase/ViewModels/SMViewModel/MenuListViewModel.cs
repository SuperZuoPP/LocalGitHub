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
    public class MenuListViewModel:NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialog;
        private readonly IMenuService service;

        public MenuListViewModel(IContainerProvider provider, IMenuService service) : base(provider)
        {
            MenuListsDtos = new ObservableCollection<TbWeighMenuDto>();
            this.regionManager = provider.Resolve<IRegionManager>();
            dialog = provider.Resolve<IDialogHostService>();
            this.service = service;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            PageUpdatedCommand = new DelegateCommand(PageUpdated);
            PerPageNumSeletedCommand = new DelegateCommand<ComboBoxItem>(PerPageNumSeleted);
            EditCommand = new DelegateCommand<TbWeighMenuDto>(Edit);
            DeleteCommand = new DelegateCommand<TbWeighMenuDto>(Delete);
        }

        #region 属性
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

        private int perPageNum = 10;

        public int PerPageNum
        {
            get { return perPageNum; }
            set { SetProperty<int>(ref perPageNum, value); }
        }
         
        private string pageSum;

        public string PageSum
        {
            get { return pageSum; }
            set { SetProperty<string>(ref pageSum, value); }
        }

        private ObservableCollection<TbWeighMenuDto> menuListsDtos;

        public ObservableCollection<TbWeighMenuDto> MenuListsDtos
        {
            get { return menuListsDtos; }
            set { SetProperty<ObservableCollection<TbWeighMenuDto>>(ref menuListsDtos, value); }
        }

        private TbWeighMenuDto currentMenuDto;

        public TbWeighMenuDto CurrentMenuDto
        {
            get { return currentMenuDto; }
            set { SetProperty<TbWeighMenuDto>(ref currentMenuDto, value); }
        }

        private ComboBoxItem comboBoxItemSelected;

        public ComboBoxItem ComboBoxItemSelected
        {
            get { return comboBoxItemSelected; }
            set { SetProperty<ComboBoxItem>(ref comboBoxItemSelected, value); }
        }

        #endregion

        #region 命令
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand PageUpdatedCommand { get; set; }

        public DelegateCommand<ComboBoxItem> PerPageNumSeletedCommand { get; set; }

        public DelegateCommand<TbWeighMenuDto> EditCommand { get; set; }

        public DelegateCommand<TbWeighMenuDto> DeleteCommand { get; set; }

        #endregion

        #region 方法
        void Execute(string obj)
        {
            switch (obj)
            {
                case "Add": Add(); break;
                case "Search": GetDataAsync(); break;
                case "SaveMenu": SaveMenu();break;
            }

        }
        private async void Edit(TbWeighMenuDto obj)
        {
            var result = await service.GetFirstOfDefaultAsync(obj.Id);
            if (result.Status)
            {
                CurrentMenuDto = result.Result; 
                IsRightDrawerOpen = true;
            }
        }

        private async void Delete(TbWeighMenuDto obj)
        {
            try
            {
                var dialogResult = await dialog.Question("温馨提示", $"确认删除菜单:{obj.MenuName} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = MenuListsDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model != null)
                        MenuListsDtos.Remove(model);
                }
            }
            catch
            {

            }
        }

        async void GetDataAsync()
        { 
            var result = await service.GetAllFilterAsync(new Shared.Parameters.TbWeighMenuDtoParameter()
            {
                PageIndex = PageIndex - 1,
                PageSize = PerPageNum,
                Search = SearchText ,
                Status = 1
            });

            if (result.Status)
            {
                MenuListsDtos.Clear();
                foreach (var item in result.Result.Items)
                {
                    MenuListsDtos.Add(item);
                }
            }
        }

        async void GetTotalSum()
        {
            try
            {
                var result = await service.GetMenuSum();
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
            CurrentMenuDto = new TbWeighMenuDto();
            IsRightDrawerOpen = true;
        }

        private void PageUpdated()
        {
            GetDataAsync(); 
        }

        private void PerPageNumSeleted(ComboBoxItem selectedItemContent)
        {
            PerPageNum = Convert.ToInt32(selectedItemContent.Content);
            GetDataAsync();
        }

        private async void SaveMenu()
        {
            if (string.IsNullOrWhiteSpace(CurrentMenuDto.MenuName) )
            {
                Growl.WarningGlobal("请输入菜单名称！"); 
                return;
            }
             
            try
            {
                if (CurrentMenuDto.Id > 0)
                { 
                    var updateResult = await service.UpdateAsync(CurrentMenuDto);
                    if (updateResult.Status)
                    {
                        var model = MenuListsDtos.FirstOrDefault(t => t.Id == CurrentMenuDto.Id);
                        if (model != null)
                        {
                            model.MenuCode = CurrentMenuDto.MenuCode;
                            model.MenuNumber = CurrentMenuDto.MenuNumber;
                            model.MenuName = CurrentMenuDto.MenuName;
                            model.Status = CurrentMenuDto.Status; 
                            Growl.SuccessGlobal("修改成功！");
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                { 
                    var addResult = await service.AddAsync(CurrentMenuDto);
                    if (addResult.Status)
                    {
                        MenuListsDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false; 
                        Growl.SuccessGlobal("添加成功！");
                    }
                }
                GetTotalSum();
            }
            catch
            {
                Growl.ErrorGlobal("添加失败！"); 
            }
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetTotalSum();
            GetDataAsync();
        }
        #endregion
    }
}
