using FastReport.Preview;
using FastReport;
using MaterialDesignColors;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using WPFBase.Models;
using WPFBase.Services;
using WPFBase.Shared.DTO.BM;
using WPFBase.ViewModels.SMViewModel;
using FastReport.Dialog;
using System.Data;

namespace WPFBase.ViewModels.BMViewModel
{ 
    public class QueryDataLineWinfromViewModel : NavigationViewModel
    {
        private readonly IDataInfoService service;
        private readonly ITbWeighWeighbridgeofficeService officeService; 
        public QueryDataLineWinfromViewModel(IContainerProvider containerProvider, IDataInfoService service, ITbWeighWeighbridgeofficeService officeService) : base(containerProvider)
        {
            this.service = service;
            this.officeService = officeService;
            WeighDataListsDtos = new ObservableCollection<TbWeighDatalineinfoDto>();
            DataInfos = new List<TbWeighDatalineinfoDto>();
            SearchCmd = new DelegateCommand(Search);  
            GetGroupList(); 
        }



        #region 属性
        
         
        private string qPlanNumber;

        public string QPlanNumber
        {
            get { return qPlanNumber; }
            set { SetProperty<string>(ref qPlanNumber, value); }
        }

        private string qCarNumber;

        public string QCarNumber
        {
            get { return qCarNumber; }
            set { SetProperty<string>(ref qCarNumber, value); }
        }

        private string qMaterialName;

        public string QMaterialName
        {
            get { return qMaterialName; }
            set { SetProperty<string>(ref qMaterialName, value); }
        }

        private DateTime queryBeginTime = Convert.ToDateTime( "2024-01-01");

        public DateTime QueryBeginTime
        {
            get { return queryBeginTime; }
            set { SetProperty<DateTime>(ref queryBeginTime, value); }
        }

        private DateTime queryEndTime = Convert.ToDateTime("2024-01-01");

        public DateTime QueryEndTime
        {
            get { return queryEndTime; }
            set { SetProperty<DateTime>(ref queryEndTime, value); }
        }

        private string qSupplierName;

        public string QSupplierName
        {
            get { return qSupplierName; }
            set { SetProperty<string>(ref qSupplierName, value); }
        }

        private string qRecipientName;

        public string QRecipientName
        {
            get { return qRecipientName; }
            set { SetProperty<string>(ref qRecipientName, value); }
        }


        private string qSelectedGroup;

        public string QSelectedGroup
        {
            get { return qSelectedGroup; }
            set { SetProperty<string>(ref qSelectedGroup, value); }
        }
         
        private ObservableCollection<TbWeighDatalineinfoDto> weighDataListsDtos;

        public ObservableCollection<TbWeighDatalineinfoDto> WeighDataListsDtos
        {
            get { return weighDataListsDtos; }
            set { SetProperty<ObservableCollection<TbWeighDatalineinfoDto>>(ref weighDataListsDtos, value); }
        }

        private List<TbWeighDatalineinfoDto> dataInfos;

        public List<TbWeighDatalineinfoDto> DataInfos
        {
            get { return dataInfos; }
            set { dataInfos = value; }
        }
         
        private ObservableCollection<PoundRoomGroup> groupList = new ObservableCollection<PoundRoomGroup>();

        public ObservableCollection<PoundRoomGroup> GroupList
        {
            get { return groupList; }
            set { SetProperty<ObservableCollection<PoundRoomGroup>>(ref groupList, value); }
        }
        #endregion

        #region 命令

        public DelegateCommand SearchCmd {  get; set; }
         
        #endregion


        #region 方法
        public async void Search()
        { 
            var results = await service.GetWeightInfoByDayRange(new Shared.Parameters.TbWeighDatalineinfoDtoParameter()
            {

                PlanCode = null,
                PlanNumber = QPlanNumber,
                WeighHouseCodes = QSelectedGroup,
                MaterialName = QMaterialName,
                SupplierName = QSupplierName,
                RecipientName = QRecipientName,
                CarNumber = QCarNumber,
                BeginWeighTime = QueryBeginTime,//DateTime.Today,
                EndWeighTime = QueryEndTime,    
                PageIndex = 0,
                PageSize = 10000, 
                Search = null,
            }) ;

            if (results.Status)
            {
                WeighDataListsDtos.Clear();
                foreach (var item in results.Result.Items)
                {
                    WeighDataListsDtos.Add(item);
                } 
            }

            //var reportFile = System.IO.Path.Join(Environment.CurrentDirectory, "Simple List.frx");
            //var report = new Report();
            //report.Load(reportFile);
            //var ds = WeighDataListsDtos;
            //report.RegisterData(ds, "WeighDataListsDtos"); 
            //report.PrepareAsync(PreviewControl);
        }

        public async Task SearchAsync()
        {
            var results = await service.GetWeightInfoByDayRange(new Shared.Parameters.TbWeighDatalineinfoDtoParameter()
            {

                PlanCode = null,
                PlanNumber = QPlanNumber,
                WeighHouseCodes = QSelectedGroup,
                MaterialName = QMaterialName,
                SupplierName = QSupplierName,
                RecipientName = QRecipientName,
                CarNumber = QCarNumber,
                BeginWeighTime = QueryBeginTime,//DateTime.Today,
                EndWeighTime = QueryEndTime,
                PageIndex = 0,
                PageSize = 10000,
                Search = null,
            });

            if (results.Status)
            {
                WeighDataListsDtos.Clear();
                foreach (var item in results.Result.Items)
                {
                    WeighDataListsDtos.Add(item);
                }
            }  
        }
        
        private async void GetGroupList()
        {

            var grouplists = await officeService.GetList();

            if (grouplists.Status)
            {
                GroupList.Clear();
                foreach (var item in grouplists.Result.Items)
                {
                    GroupList.Add(new PoundRoomGroup()
                    {
                        GroupId = item.WeighHouseCode,
                        GroupName = item.WeighHouseName
                    });
                }
            }
        }

        #endregion
    }


    
}
 