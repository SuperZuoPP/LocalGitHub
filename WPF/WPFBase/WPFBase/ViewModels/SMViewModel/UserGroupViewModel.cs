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

         async void OpenAddDiaolog(string obj) 
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value", "Hello"); //发送数据至弹窗 
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

        private string search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
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
                PageIndex = 0,
                PageSize = 5,
                Search = Search, 
                Status = 1
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
    }
     
}
