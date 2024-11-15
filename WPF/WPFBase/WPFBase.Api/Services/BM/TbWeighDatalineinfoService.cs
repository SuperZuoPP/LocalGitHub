﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;
using WPFBase.Api.Extensions;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WPFBase.Shared.Extensions;
using System.Threading.Tasks.Dataflow;

namespace WPFBase.Api.Services.BM
{
    public class TbWeighDatalineinfoService : ITbWeighDatalineinfoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TbWeighDatalineinfoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(TbWeighDatalineinfoDto modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<tb_weigh_datalineinfo>(modeldto);
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(modeldto.Id));
                if (model == null)
                {
                    dbmodel.CreateTime = DateTime.Now;
                    dbmodel.WeighRecordNumber = SystemBase.GetRndStrOnlyFor(20, true);
                    await unitOfWork.GetRepository<tb_weigh_datalineinfo>().InsertAsync(dbmodel);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, dbmodel);
                }

                return new ApiResponse(false, "添加数据失败，明细已存在");

            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                if (model != null) 
                {
                    model.OperateBit = 2;//假删除
                    repository.Update(model);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, model);
                }
                return new ApiResponse(false, "删除数据失败！");
               
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
             
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var models = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(query.Search) ? true : x.CarNumber.Contains(query.Search),
                    pageIndex: query.PageIndex,
                    pageSize: query.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
         

        public async Task<ApiResponse> UpdateAsync(TbWeighDatalineinfoDto modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<tb_weigh_datalineinfo>(modeldto);
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbmodel.Id));
                model.LastModifiedTime = DateTime.Now;
                repository.Update(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                return new ApiResponse(false, "更新数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetWeightInfoByDay(TbWeighDatalineinfoDtoParameter parameter)
        {
            try
            {
                DateTime timein = parameter.WeighTime == default ? DateTime.Today : Convert.ToDateTime(parameter.WeighTime.ToString("yyyy-MM-dd"));
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var models = await repository.GetPagedListAsync(predicate: x => x.OperateBit != 2
                    && (x.WeighTime != null && x.WeighTime.Value.Date == timein)
                    && (string.IsNullOrWhiteSpace(parameter.PlanNumber) || x.CarNumber.Contains(parameter.PlanNumber))
                    && (string.IsNullOrWhiteSpace(parameter.CarNumber) || x.CarNumber.Contains(parameter.CarNumber))
                    && (string.IsNullOrWhiteSpace(parameter.SupplierName) || x.SupplierName.Contains(parameter.SupplierName))
                    && (string.IsNullOrWhiteSpace(parameter.RecipientName) || x.RecipientName.Contains(parameter.RecipientName))
                    && (string.IsNullOrWhiteSpace(parameter.MaterialName) || x.MaterialName.Contains(parameter.MaterialName))
                    && ((string.IsNullOrWhiteSpace(parameter.WeighHouseCodes) || x.GrossWeighHouseCode.Contains(parameter.WeighHouseCodes)) ||
                    (string.IsNullOrWhiteSpace(parameter.WeighHouseCodes) || x.TareWeighHouseCode.Contains(parameter.WeighHouseCodes)))
                    ,
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                    orderBy: source => source.OrderBy(t => t.WeighTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }


        public async Task<ApiResponse> GetWeightInfoByDayRange(TbWeighDatalineinfoDtoParameter parameter)
        {
            try
            {
                DateTime timebegin = parameter.BeginWeighTime == default ? DateTime.Today : Convert.ToDateTime(parameter.BeginWeighTime.ToString("yyyy-MM-dd"));
                DateTime timeend = parameter.EndWeighTime == default ? DateTime.Today : Convert.ToDateTime(parameter.EndWeighTime.ToString("yyyy-MM-dd"));
                var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
                var models = await repository.GetPagedListAsync(predicate: x => x.OperateBit != 2
                    && (x.WeighTime != null && x.WeighTime.Value.Date >= timebegin && x.WeighTime.Value.Date <= timeend)
                    && (string.IsNullOrWhiteSpace(parameter.PlanNumber) || x.CarNumber.Contains(parameter.PlanNumber))
                    && (string.IsNullOrWhiteSpace(parameter.CarNumber) || x.CarNumber.Contains(parameter.CarNumber))
                    && (string.IsNullOrWhiteSpace(parameter.SupplierName) || x.SupplierName.Contains(parameter.SupplierName))
                    && (string.IsNullOrWhiteSpace(parameter.RecipientName) || x.RecipientName.Contains(parameter.RecipientName))
                    && (string.IsNullOrWhiteSpace(parameter.MaterialName) || x.MaterialName.Contains(parameter.MaterialName))
                    && ((string.IsNullOrWhiteSpace(parameter.WeighHouseCodes) || x.GrossWeighHouseCode.Contains(parameter.WeighHouseCodes)) ||
                    (string.IsNullOrWhiteSpace(parameter.WeighHouseCodes) || x.TareWeighHouseCode.Contains(parameter.WeighHouseCodes)))
                    ,
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                    orderBy: source => source.OrderBy(t => t.WeighTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetWeightInfo(QueryParameter parameter)
        {
            try
            {
                var repository1 = unitOfWork.GetRepository<tb_weigh_plan>();
                var repository2 = unitOfWork.GetRepository<tb_weigh_datalineinfo>();  
                var query = from aa in repository1.GetAll()
                            join bb in repository2.GetAll()
                            on aa.PlanCode equals bb.PlanCode
                            where bb.PlanCode == parameter.Search
                            && bb.OperateBit != 2
                            select new tb_weigh_datalineinfo
                            {
                                Id = bb.Id,
                                CarNumber = bb.CarNumber,
                                PlanNumber = bb.PlanNumber, 
                                CreateTime = bb.CreateTime
                            };
                
                var paginatedQuery = query
               .Skip((parameter.PageIndex - 1) * parameter.PageSize)
               .Take(parameter.PageSize);
                 
                var resultList = await paginatedQuery.ToListAsync();
                return new ApiResponse(true, resultList);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }

            //try
            //{
            //    var repository = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
            //    var models = await repository.GetPagedListAsync(predicate: x => x.OperateBit != 2,
            //        pageIndex: parameter.PageIndex,
            //        pageSize: parameter.PageSize,
            //   orderBy: source => source.OrderByDescending(t => t.CreateTime));

            //    return new ApiResponse(true, models);

            //}
            //catch (Exception ex)
            //{
            //    return new ApiResponse(ex.Message);
            //} 
        }

        //public async Task<ApiResponse> GetWeightInfoByDay(TbWeighDatalineinfoDtoParameter parameter)
        //{
        //    try
        //    {
        //        var rplittlrplan = unitOfWork.GetRepository<tb_weigh_littleplan>();
        //        var rpinfo = unitOfWork.GetRepository<tb_weigh_datalineinfo>();
        //        var rpplan = unitOfWork.GetRepository<tb_weigh_plan>();
        //        var query = from little in rplittlrplan.GetAll()
        //                    join info in rpinfo.GetAll() on little.QrCode equals info.Attribute2 into infoGroup
        //                    from info in infoGroup.DefaultIfEmpty()
        //                    join plan in rpplan.GetAll() on info.PlanCode equals plan.PlanCode into planinfoGroup
        //                    from plan in planinfoGroup.DefaultIfEmpty()
        //                    where info.OperateBit != 2
        //                    && (string.IsNullOrWhiteSpace(parameter.CarNumber) || info.CarNumber.Contains(parameter.CarNumber))
        //                    && (string.IsNullOrWhiteSpace(parameter.SupplierName) || info.SupplierName.Contains(parameter.SupplierName))
        //                    && (string.IsNullOrWhiteSpace(parameter.RecipientName) || info.RecipientName.Contains(parameter.RecipientName))
        //                    && (string.IsNullOrWhiteSpace(parameter.MaterialName) || info.MaterialName.Contains(parameter.MaterialName))
        //                    && (info.WeighTime != null && info.WeighTime.Value.Date == parameter.WeighTime.Date)
        //                    orderby
        //                    (info.GrossWeighTime == null || info.TareWeighTime == null) ? 0 :
        //                    (info.GrossWeighTime != null && info.TareWeighTime != null) ? 1 :
        //                    0 ascending,
        //                    (info.GrossWeighTime == null || info.TareWeighTime == null) ? info.WeighTime :
        //                    info.WeighTime descending
        //                    select new
        //                    {
        //                        QrCode = little.QrCode,
        //                        CarNumber = little.CarNo,
        //                        IsWeight = little.IsWeight,
        //                        DriverName = little.DriverName
        //                    };

        //        var models = await query.ToListAsync();
        //        return new ApiResponse(true, models);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse(ex.Message);
        //    }
        //}
    }
}
