using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.UnitOfWork;
using WPFBase.Api.Extensions;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Extensions;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public class TbWeighUsergroupService:ITbWeighUsergroupService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TbWeighUsergroupService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(TbWeighUsergroupDto modeldto)
        {
            try
            { 
                var dbmodel = mapper.Map<TbWeighUsergroup>(modeldto);
                var repository = unitOfWork.GetRepository<TbWeighUsergroup>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.UserGroupName.Equals(modeldto.UserGroupName));
                if (model == null)
                {
                    dbmodel.CreateTime = DateTime.Now;
                    dbmodel.UserGroupCode = SystemBase.GetRndStrOnlyFor(20, true); 
                    await unitOfWork.GetRepository<TbWeighUsergroup>().InsertAsync(dbmodel);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, dbmodel);
                }

                return new ApiResponse(false, "添加数据失败，用户组已存在");

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
                var repository = unitOfWork.GetRepository<TbWeighUsergroup>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
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
                var repository = unitOfWork.GetRepository<TbWeighUsergroup>();
                var models = await repository.GetPagedListAsync(predicate:x=> string.IsNullOrWhiteSpace(query.Search) ? true : x.UserGroupName.Contains(query.Search),
                    pageIndex:query.PageIndex,
                    pageSize:query.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public  async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighUsergroup>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, model); 
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
         

        public async Task<ApiResponse> UpdateAsync(TbWeighUsergroupDto modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<TbWeighUsergroup>(modeldto);
                var repository = unitOfWork.GetRepository<TbWeighUsergroup>();
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
        /// 用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ApiResponse> GetUserList(TbWeighOperatorDtoParameter paramter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighOperator>();
                var models = await repository.GetPagedListAsync( x => (string.IsNullOrWhiteSpace(paramter.Search) ? true : x.UserName.Contains(paramter.Search))
                   && (!paramter.Status.HasValue || x.Status == (paramter.Status == 1)),
                    pageIndex: paramter.PageIndex,
                    pageSize: paramter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetUserSum()
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighOperator>();
                var count = await repository.CountAsync();
                return new ApiResponse(true, count);
            }
            catch (Exception ex)
            {
                return new ApiResponse("获取总人员数失败！" + ex.ToString());
            }
        }

        public async Task<ApiResponse> GetUserGroupAndUserList(QueryParameter query)
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighGroupauthorityuser>();
                var models = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(query.Search) ? true : x.UserGroupCode.Equals(query.Search),
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

        public async Task<ApiResponse> GroupUserAdd(TbWeighGroupauthorityuserDto groupauthorityuserDto)
        {
            try
            { 
                var dbmodel = mapper.Map<TbWeighGroupauthorityuser>(groupauthorityuserDto);
                var repository = unitOfWork.GetRepository<TbWeighGroupauthorityuser>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => (x.UserCode.Equals(groupauthorityuserDto.UserCode) && x.UserGroupCode.Equals(groupauthorityuserDto.UserGroupCode) ));
                if (model == null)
                {
                    dbmodel.CreateTime = DateTime.Now;
                    dbmodel.UserGroupCode = groupauthorityuserDto.UserGroupCode;
                    dbmodel.UserCode = groupauthorityuserDto.UserCode;
                    dbmodel.OperateBit = 0;
                    await repository.InsertAsync(dbmodel);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, dbmodel);
                }
                return new ApiResponse(false, "添加数据失败，用户已存在该组中！");
                  
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GroupUserRemove(TbWeighGroupauthorityuserDto groupauthorityuserDto)
        {
            try
            {
                var dbmodel = mapper.Map<TbWeighGroupauthorityuser>(groupauthorityuserDto);
                var repository = unitOfWork.GetRepository<TbWeighGroupauthorityuser>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => (x.UserCode.Equals(groupauthorityuserDto.UserCode) && x.UserGroupCode.Equals(groupauthorityuserDto.UserGroupCode)));
                repository.Delete(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
             
        }
    }
}
