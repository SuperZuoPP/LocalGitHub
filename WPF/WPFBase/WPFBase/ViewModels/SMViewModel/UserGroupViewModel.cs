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

namespace WPFBase.ViewModels.SMViewModel
{
    public class UserGroupViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialog;
        private readonly ILoginService loginService;

        public UserGroupViewModel(IContainerProvider provider, IDialogHostService dialog,ILoginService loginService) : base(provider)
        {
            WeighOperatorDto = new ObservableCollection<TbWeighOperatorDto>(); 
            this.regionManager = provider.Resolve<IRegionManager>();
            this.dialog = dialog;
            this.loginService = loginService;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            EditCommand = new DelegateCommand<TbWeighOperatorDto>(Edit);
            DeleteCommand = new DelegateCommand<TbWeighOperatorDto>(Delete);
            GetTotalSum();
            GetDataAsync();
        }

        void Execute(string obj)
        {
            switch (obj)
            {
                case "UserCreateView":
                    OpenAddDiaolog(obj);
                    break;
                case "search":
                    GetDataAsync();
                    break;
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
                    var model = WeighOperatorDto.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model != null)
                        WeighOperatorDto.Remove(model);
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
                OpenAddDiaolog("UserCreateView");
            }
        }

        async void OpenAddDiaolog(string obj) 
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value", CurrentTbWeighOperatorDto); //发送数据至弹窗 
            var dialogResult = await dialog.ShowDialog(obj, keys);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    aggregator.SendMessage("添加成功!");
                }
                catch
                {

                }
            }
        }
       
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<TbWeighOperatorDto> EditCommand { get; set; }

        public DelegateCommand<TbWeighOperatorDto> DeleteCommand { get; set; }

        private ObservableCollection<TbWeighOperatorDto> weighOperatorDto;

        public ObservableCollection<TbWeighOperatorDto> WeighOperatorDto
        {
            get { return weighOperatorDto; }
            set { SetProperty<ObservableCollection<TbWeighOperatorDto>>(ref weighOperatorDto, value); }
        }

        private TbWeighOperatorDto currentTbWeighOperatorDto;

        public TbWeighOperatorDto CurrentTbWeighOperatorDto
        {
            get { return currentTbWeighOperatorDto; }
            set { SetProperty<TbWeighOperatorDto>(ref currentTbWeighOperatorDto, value); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty<string>(ref title, value); }
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

        private int pageIndex=1;

        public int PageIndex
        {
            get { return pageIndex; }
            set { SetProperty<int>(ref pageIndex, value); GetDataAsync(); }
        }

        private int pageSum;

        public int PageSum
        {
            get { return pageSum; }
            set { SetProperty<int>(ref pageSum, value); GetDataAsync(); }
        }

        private ICollectionView dataGridCollectionView;
        public ICollectionView DataGridCollectionView
        {
            get { return dataGridCollectionView; }
            set { SetProperty<ICollectionView>(ref dataGridCollectionView, value); }
        }
       

        async void GetDataAsync() 
        {
            var result = await loginService.GetAllFilterAsync(new Shared.Parameters.TbWeighOperatorDtoParameter() {
                PageIndex = PageIndex-1,
                PageSize = 10,
                Search = SearchText, 
                Status = Status
            });

            if (result.Status)
            {
                WeighOperatorDto.Clear();
                foreach (var item in result.Result.Items)
                {
                    WeighOperatorDto.Add(item);
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
                    PageSum = (int)Math.Ceiling(Convert.ToDouble(result.Result.ToString())/10);
                }
            }
            catch  
            { 

            }
        }
    }
     
}
