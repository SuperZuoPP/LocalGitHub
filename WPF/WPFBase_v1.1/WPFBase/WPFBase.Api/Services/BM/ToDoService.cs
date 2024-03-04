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

namespace WPFBase.Api.Services.BM
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate:
                   x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                   pageIndex: parameter.PageIndex,
                   pageSize: parameter.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate:
                   x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search))
                   && (parameter.Status == null ? true : x.Status.Equals(parameter.Status)),
                   pageIndex: parameter.PageIndex,
                   pageSize: parameter.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateTime));
                return new ApiResponse(true, todos);
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
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(model);
                todo.CreateTime = DateTime.Now;
                await unitOfWork.GetRepository<ToDo>().InsertAsync(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse(false, "添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> Summary()
        {
            try
            {
                //待办事项结果
                var todos = await unitOfWork.GetRepository<ToDo>().GetAllAsync(
                    orderBy: source => source.OrderByDescending(t => t.CreateTime));

                //备忘录结果
                var memos = await unitOfWork.GetRepository<Memo>().GetAllAsync(
                    orderBy: source => source.OrderByDescending(t => t.CreateTime));

                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count(); //汇总待办事项数量
                summary.CompletedCount = todos.Where(t => t.Status == 1).Count(); //统计完成数量
                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%"); //统计完成率
                summary.MemoeCount = memos.Count();  //汇总备忘录数量
                summary.ToDoList = new ObservableCollection<ToDoDto>(mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
                summary.MemoList = new ObservableCollection<MemoDto>(mapper.Map<List<MemoDto>>(memos));

                return new ApiResponse(true, summary);
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "");
            }
        }
        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var dbtodo = mapper.Map<ToDo>(model);
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbtodo.Id));
                todo.Title = dbtodo.Title;
                todo.Content = dbtodo.Content;
                todo.Status = dbtodo.Status;
                todo.LastModifiedTime = DateTime.Now;
                repository.Update(todo);

                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
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
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(todo);
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

