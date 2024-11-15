﻿using MaterialDesignColors;
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

namespace WPFBase.ViewModels.BMViewModel
{ 
    public class DataInfoViewModel : NavigationViewModel
    {
        private readonly IDataInfoService service;
        private readonly ITbWeighWeighbridgeofficeService officeService;
        private readonly ICollectionView view;
        private readonly PropertyGroupDescription groupDescription;
        public DataInfoViewModel(IContainerProvider containerProvider, IDataInfoService service, ITbWeighWeighbridgeofficeService officeService) : base(containerProvider)
        {
            this.service = service;
            this.officeService = officeService;
            WeighDataListsDtos = new ObservableCollection<TbWeighDatalineinfoDto>();
            view = CollectionViewSource.GetDefaultView(WeighDataListsDtos);
            groupDescription = new PropertyGroupDescription("PlanNumber");
            SearchCmd = new DelegateCommand(Search);
            SearchGroupCmd = new DelegateCommand(SearchGroup);
            PerPageNumSeletedCommand = new DelegateCommand<ComboBoxItem>(PerPageNumSeleted);
            PageUpdatedCommand = new DelegateCommand(PageUpdated);
            // GetGroupList();
            GroupList = AppSession.PoundRoomGroupList;
        }

       

        #region 属性
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

        private int perPageNum = 100;

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

        private bool isCheckedGroup;

        public bool IsCheckedGroup
        {
            get { return isCheckedGroup; }
            set { SetProperty<bool>(ref isCheckedGroup, value); }
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

        private DateTime queryTime=DateTime.Now.AddMonths(-11);

        public DateTime QueryTime 
        {
            get { return queryTime; }
            set { SetProperty<DateTime>(ref queryTime, value); }
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


        private ComboBoxItem comboBoxItemSelected;

        public ComboBoxItem ComboBoxItemSelected
        {
            get { return comboBoxItemSelected; }
            set { SetProperty<ComboBoxItem>(ref comboBoxItemSelected, value); }
        }

        private ObservableCollection<TbWeighDatalineinfoDto> weighDataListsDtos;

        public ObservableCollection<TbWeighDatalineinfoDto> WeighDataListsDtos
        {
            get { return weighDataListsDtos; }
            set { SetProperty<ObservableCollection<TbWeighDatalineinfoDto>>(ref weighDataListsDtos, value); }
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

        public DelegateCommand SearchGroupCmd { get; set; } 

        public DelegateCommand<ComboBoxItem> PerPageNumSeletedCommand { get; set; }

        public DelegateCommand PageUpdatedCommand { get; set; }
        #endregion


        #region 方法
        private async void Search()
        { 
            var results = await service.GetWeightInfoByDay(new Shared.Parameters.TbWeighDatalineinfoDtoParameter()
            {

                PlanCode = null,
                PlanNumber = null,
                WeighHouseCodes = QSelectedGroup,
                MaterialName = QMaterialName,
                SupplierName = QSupplierName,
                RecipientName = QRecipientName,
                CarNumber = QCarNumber,
                WeighTime = QueryTime,//DateTime.Today,
                PageIndex = PageIndex - 1,
                PageSize = PerPageNum, 
                Search = null,
            }) ;

            if (results.Status)
            {
                WeighDataListsDtos.Clear();
                foreach (var item in results.Result.Items)
                {
                    WeighDataListsDtos.Add(item);
                }
                PageCount = results.Result.TotalPages;
                PageSum = "共 "+results.Result.TotalCount.ToString()+" 条";

               
                //foreach (var group in view.Groups)
                //{ 
                //    double totalWeigh = 0;
                //    CollectionViewGroup collectionViewGroup = group as CollectionViewGroup;
                //    if (collectionViewGroup != null)
                //    {
                //        foreach (TbWeighDatalineinfoDto tbWeigh in collectionViewGroup.Items)
                //        {
                //            totalWeigh += tbWeigh.Suttle;
                //        } 
                //    }
                //}

            }


        }

        private void SearchGroup()
        {
            if (IsCheckedGroup)
            { 
                view.GroupDescriptions.Add(groupDescription);  
            }
            else
            {
                view.GroupDescriptions.Clear(); 
            } 
        }


        private void PerPageNumSeleted(ComboBoxItem selectedItemContent)
        {
            PerPageNum = Convert.ToInt32(selectedItemContent.Content);
            Search();
        }

        private void PageUpdated()
        {
            Search();
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
 