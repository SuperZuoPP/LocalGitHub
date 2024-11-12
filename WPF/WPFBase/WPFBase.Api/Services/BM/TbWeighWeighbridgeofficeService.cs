using AutoMapper;
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

namespace WPFBase.Api.Services.BM
{
    public class TbWeighWeighbridgeofficeService : ITbWeighWeighbridgeofficeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TbWeighWeighbridgeofficeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(TbWeighWeighbridgeofficeDTO modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<tb_weigh_weighbridgeoffice>(modeldto);
                var repository = unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(modeldto.Id));
                if (model == null)
                {
                    dbmodel.CreateTime = DateTime.Now;
                    dbmodel.WeighHouseCode = SystemBase.GetRndStrOnlyFor(20, true);
                    await unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>().InsertAsync(dbmodel);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, dbmodel);
                }

                return new ApiResponse(false, "添加数据失败");

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
                var repository = unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>();
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
                var repository = unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>();
                var models = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(query.Search) ? true : x.WeighHouseName.Contains(query.Search),
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
                var repository = unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
         

        public async Task<ApiResponse> UpdateAsync(TbWeighWeighbridgeofficeDTO modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<tb_weigh_weighbridgeoffice>(modeldto);
                var repository = unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>();
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


        /// <summary>
        /// 获取磅房列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetList()
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_weighbridgeoffice>();
                var models = await repository.GetPagedListAsync(predicate: x =>x.OperateBit!=2 && x.Status==true,
                    orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
