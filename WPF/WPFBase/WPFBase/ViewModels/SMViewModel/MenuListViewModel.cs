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
        #endregion

        #region 命令
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        #endregion

        #region 方法
        void Execute(string obj)
        {
            switch (obj)
            {
                case "Add": Add(); break;
                case "Search": GetDataAsync(); break; 
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext); 
        }
        #endregion
    }
}
