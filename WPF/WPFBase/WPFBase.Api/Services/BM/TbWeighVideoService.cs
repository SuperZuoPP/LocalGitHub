using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.UnitOfWork;
using WPFBase.Api.Extensions;
using WPFBase.Api.Services.SM; 
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFBase.Api.Services.BM
{
    public class TbWeighVideoService : ITbWeighVideoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TbWeighVideoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(TbWeighVideoDto modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<TbWeighVideo>(modeldto);
                var repository = unitOfWork.GetRepository<TbWeighVideo>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(modeldto.Id));
                if (model == null)
                {
                    dbmodel.CreateTime = DateTime.Now; 
                    await unitOfWork.GetRepository<TbWeighVideo>().InsertAsync(dbmodel);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, dbmodel);
                }

                return new ApiResponse(false, "添加数据失败，已存在!");

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
                var repository = unitOfWork.GetRepository<TbWeighVideo>();
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
                var repository = unitOfWork.GetRepository<TbWeighVideo>();
                var models = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(query.Search) ? true : x.Ip.Contains(query.Search),
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
                var repository = unitOfWork.GetRepository<TbWeighVideo>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
         

        public async Task<ApiResponse> UpdateAsync(TbWeighVideoDto modeldto)
        {
            try
            {
                var dbmodel = mapper.Map<TbWeighVideo>(modeldto);
                var repository = unitOfWork.GetRepository<TbWeighVideo>();
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

        public async Task<ApiResponse> GetVideoList(TbWeighVideoDtoParameter parameter)
        {
            //方法一
            //try
            //{  
            //    var repository1 = unitOfWork.GetRepository<TbWeighVideo>();
            //    var repository2 = unitOfWork.GetRepository<TbWeighDevicestatus>();

            //    string videoTypeNo = parameter.VideoTypeNo.ToString();
            //    int status = parameter.Status;

            //    var query = from t1 in repository1.GetAll()
            //                join t2 in repository2.GetAll()
            //                on t1.Attribute1 equals t2.SlaveDeviceNo into statusGroup
            //                from t2 in statusGroup.DefaultIfEmpty()
            //                where t1.VideoTypeNo == videoTypeNo
            //                && (t1.OperateBit != 2 || t1.OperateBit == null)
            //                && (t2.Status == status || t2.Status == null)
            //                select new
            //                {
            //                    t1.VideoTypeNo,
            //                    t1.Ip,
            //                    t1.Port,
            //                    t1.UserName,
            //                    t1.PassWord,
            //                    t1.Channelnub,
            //                    t1.Position,
            //                    t1.Status,
            //                    t1.WeighHouseCodes,
            //                    t2.SlaveDeviceName,
            //                    t2.SlaveDeviceNo,
            //                    t2.SlaveDeviceType
            //                };

            //    var result = await query.ToListAsync();
            //    return new ApiResponse(true, result); 
            //}
            //catch (Exception ex)
            //{
            //    return new ApiResponse(ex.Message);
            //}

            //方法二，分步查
            try
            {
                var repository1 = unitOfWork.GetRepository<TbWeighVideo>();
                var repository2 = unitOfWork.GetRepository<TbWeighDevicestatus>();

                string videoTypeNo = parameter.VideoTypeNo.ToString();
                int status = parameter.Status;

                // 查询 repository1 中满足条件的数据
                var filteredVideos = await repository1.GetAll()
                    .Where(t1 => t1.VideoTypeNo == videoTypeNo && (t1.OperateBit != 2 || t1.OperateBit == null))
                    .ToListAsync();

                // 根据 filteredVideos 中的 Attribute1 查询 repository2 中的数据
                var query = from t1 in filteredVideos
                            join t2 in repository2.GetAll() on t1.Attribute1 equals t2.SlaveDeviceNo into statusGroup
                            from t2 in statusGroup.DefaultIfEmpty()
                            where t2.Status == status || t2.Status == null
                            select new
                            {
                                t1.VideoTypeNo,
                                t1.Ip,
                                t1.Port,
                                t1.UserName,
                                t1.PassWord,
                                t1.Channelnub,
                                t1.Position,
                                t1.Status,
                                t1.WeighHouseCodes,
                                t2.SlaveDeviceName,
                                t2.SlaveDeviceNo,
                                t2.SlaveDeviceType
                            };
        //         private string factory;
        //private string model;
        //private string videoType;
        //private string videoTypeNo;
        //private string iP;
        //private string iPHistory;
        //private string port;
        //private double userName;
        //private double passWord;
        //private double channelnub;
        //private string storage;
        //private string pOSITION;
        //private string status;
        //private string weighHouseCodes;
                var result = query.ToList();
                return new ApiResponse(true, result);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

       
    }
}
