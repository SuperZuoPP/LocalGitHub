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
            ListKind = new ObservableCollection<string>() { "Cog", "Account", "AccountCog", "AccountMultiple", "Airplane", "Alarm", "Alert", "AlertCircleOutline"
            ,"AlignHorizontalLeft","AlphaZCircle","Antenna","AppleKeyboardCommand","Apps","ArrowDownDropCircleOutline","ArrowUPDropCircleOutline","BarcodeScan"
            ,"BadgeAccountHorizontalOutline","BellOutline","BoomGate","BoomGateDown","BoomGateUp","Brightness4","Brightness5","Bus"};
            MenuListsDtos = new ObservableCollection<TbWeighMenuDto>();
            MenuListsCombobox = new ObservableCollection<TbWeighMenuDto>();  
            this.regionManager = provider.Resolve<IRegionManager>();
            dialog = provider.Resolve<IDialogHostService>();
            this.service = service;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            PageUpdatedCommand = new DelegateCommand(PageUpdated);
            PerPageNumSeletedCommand = new DelegateCommand<ComboBoxItem>(PerPageNumSeleted);
            EditCommand = new DelegateCommand<TbWeighMenuDto>(Edit);
            DeleteCommand = new DelegateCommand<TbWeighMenuDto>(Delete);
            SelectedGroupCommand = new DelegateCommand<TbWeighMenuDto>(SelectComboBoxItem);
            SelectIconCmd = new DelegateCommand<string>(SelectListBoxItem);
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

        private string selectKind;

        public string SelectKind
        {
            get { return selectKind; }
            set { SetProperty<string>(ref selectKind, value); }
        }
        

        private ObservableCollection<TbWeighMenuDto> menuListsDtos;

        public ObservableCollection<TbWeighMenuDto> MenuListsDtos
        {
            get { return menuListsDtos; }
            set { SetProperty<ObservableCollection<TbWeighMenuDto>>(ref menuListsDtos, value); }
        }
        
        private ObservableCollection<string> listKind;

        public ObservableCollection<string> ListKind
        {
            get { return listKind; }
            set { SetProperty<ObservableCollection<string>>(ref listKind, value); }
        }

        private ObservableCollection<TbWeighMenuDto> menuListsCombobox;

        public ObservableCollection<TbWeighMenuDto> MenuListsCombobox
        {
            get { return menuListsCombobox; }
            set { SetProperty<ObservableCollection<TbWeighMenuDto>>(ref menuListsCombobox, value); }
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

        private object selectedMenuId;

        public object SelectedMenuId
        {
            get { return selectedMenuId; }
            set { SetProperty<object>(ref selectedMenuId, value); }
        }
        #endregion

        #region 命令
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand PageUpdatedCommand { get; set; }

        public DelegateCommand<ComboBoxItem> PerPageNumSeletedCommand { get; set; }

        public DelegateCommand<TbWeighMenuDto> EditCommand { get; set; }

        public DelegateCommand<TbWeighMenuDto> DeleteCommand { get; set; }
         
        public DelegateCommand<TbWeighMenuDto> SelectedGroupCommand { get; set; }

        public DelegateCommand<string> SelectIconCmd { get; set; }
        

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

        private async void Delete(TbWeighMenuDto dto)
        { 
            try
            {
                var dialogResult = await dialog.Question("温馨提示", $"确认删除菜单:{dto.MenuName} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                var deleteResult = await service.DeleteAsync(dto.Id);
                if (deleteResult.Status)
                {
                    var model = MenuListsDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
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
                Status = null
            });

            if (result.Status)
            {
                MenuListsDtos.Clear();
                foreach (var item in result.Result.Items)
                {
                    MenuListsDtos.Add(item);
                }
            }

            var resultCombobox = await service.GetAllFilterAsync(new Shared.Parameters.TbWeighMenuDtoParameter()
            {
                PageIndex = 0,
                PageSize = 1000,
                Search = "",
                Status = 1
            });

            if (resultCombobox.Status)
            {
                MenuListsCombobox.Clear();
                foreach (var item in resultCombobox.Result.Items)
                {
                    MenuListsCombobox.Add(item);
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
                            model.Attribute1 = CurrentMenuDto.Attribute1;//父菜单名称
                            model.Attribute2 = CurrentMenuDto.Attribute2;//菜单图标
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
                GetDataAsync();
            }
            catch
            {
                Growl.ErrorGlobal("添加失败！"); 
            }
        }

        /// <summary>
        /// 当前选中图标
        /// </summary>
        /// <param name="item"></param>
        private void SelectListBoxItem(string item)
        {
            SelectKind = item;
            CurrentMenuDto.Attribute2 = item; //图标
        }


        /// <summary>
        /// 当前选中菜单
        /// </summary>
        /// <param name="item"></param>
        private void SelectComboBoxItem(TbWeighMenuDto item)
        {
            if (item == null) return;
            CurrentMenuDto.Attribute1 = item.MenuName;
            currentMenuDto.MenuNumber = item.Id.ToString();
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
