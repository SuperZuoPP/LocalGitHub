using AutoMapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;
using WPFBase.Api.Context.UnitOfWork;
using System.Linq;
using WPFBase.Api.Context.Model.BM;
using System.Reflection.Metadata;

namespace WPFBase.Api.Services.BM
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MenuService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighMenu>();
                var models = await repository.GetPagedListAsync(predicate:
                   x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.MenuName.Contains(parameter.Search),
                   pageIndex: parameter.PageIndex,
                   pageSize: parameter.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, models);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllFilterAsync(TbWeighMenuDtoParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighMenu>();
                var models = await repository.GetPagedListAsync(predicate:
                   x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.MenuName.Contains(parameter.Search))
                   && (!parameter.Status.HasValue || x.Status == (parameter.Status == 1)),
                   pageIndex: parameter.PageIndex,
                   pageSize: parameter.PageSize,
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
                var repository = unitOfWork.GetRepository<TbWeighMenu>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> AddAsync(TbWeighMenuDto parameter)
        {
            try
            {
                var model = mapper.Map<TbWeighMenu>(parameter);
                model.CreateTime = DateTime.Now;
                await unitOfWork.GetRepository<TbWeighMenu>().InsertAsync(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                return new ApiResponse(false, "添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetMenuSum()
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighMenu>();
                var count = await repository.CountAsync();
                return new ApiResponse(true, count);
            }
            catch (Exception ex)
            {
                return new ApiResponse("获取总菜单数失败！" + ex.ToString());
            }
        }
        public async Task<ApiResponse> UpdateAsync(TbWeighMenuDto parameter)
        {
            try
            {
                var dbmodel = mapper.Map<TbWeighMenu>(parameter);
                var repository = unitOfWork.GetRepository<TbWeighMenu>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbmodel.Id));
                model.MenuCode = dbmodel.MenuCode;
                model.MenuNumber = dbmodel.MenuNumber;
                model.MenuName = dbmodel.MenuName;
                model.Status = dbmodel.Status;
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

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<TbWeighMenu>();
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


    }
}

