﻿using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model;
using WPFBase.Entities.BM;
using WPFBase.Entities.SM;
using WPFBase.Api.Context.UnitOfWork;
using WPFBase.Api.Extensions;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared.Extensions;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public class TbWeighOperatorService : ITbWeighOperatorService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TbWeighOperatorService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> GetAllFilterAsync(TbWeighOperatorDtoParameter paramter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var operators = await repository.GetPagedListAsync(predicate:
                   x => (string.IsNullOrWhiteSpace(paramter.Search) ? true : x.UserName.Contains(paramter.Search))
                   && (!paramter.Status.HasValue || x.Status == (paramter.Status == 1)),//(paramter.Status == null ? true : x.Status.Equals(paramter.Status)),
                   pageIndex: paramter.PageIndex,
                   pageSize: paramter.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateTime));
 
                return new ApiResponse(true, operators);
            }
            catch (Exception ex)
            { 
                return new ApiResponse(ex.Message);
            }
             
        }

        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try
            {
                password = EncryptTools.GetMD5(account+password);
                var model = await  unitOfWork.GetRepository<tb_weigh_operator>().GetFirstOrDefaultAsync(predicate: x => (x.UserNumber.Equals(account)) && (x.Password.Equals(password)));
                if (model == null)
                    return new ApiResponse("账号密码错误！请重新输入！");

                return new ApiResponse(true, new OperatorDto()
                {
                    UserCode = model.UserCode,
                    UserNumber = model.UserNumber,
                    UserName = model.UserName,
                    Id = model.Id
                });
            }
            catch (Exception)
            {
                return new ApiResponse(false, "登录失败！");
            }
        }

        public async Task<ApiResponse> Resgiter(TbWeighOperatorDto operatorDto)
        {
            try
            {
                var model = mapper.Map<tb_weigh_operator>(operatorDto);
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var operatormodel = await repository.GetFirstOrDefaultAsync(predicate: x => x.UserNumber.Equals(model.UserNumber));
                if (operatormodel != null)
                    return new ApiResponse($"当前账号:{model.UserNumber}已存在,请重新注册！");
                 
                model.UserCode = SystemBase.GetRndStrOnlyFor(20, true); 
                model.CreateTime = DateTime.Now;
                model.Password = EncryptTools.GetMD5(model.UserNumber+model.Password);
                await repository.InsertAsync(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);

                return new ApiResponse("注册失败,请稍后重试！");
            }
            catch (Exception ex)
            {
                return new ApiResponse("注册账号失败！" + ex.ToString());
            }
        }


        public async Task<ApiResponse> Summary()
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var count = await repository.CountAsync();  
                return new ApiResponse(true, count);
            }
            catch (Exception ex)
            {
                return new ApiResponse("获取总人员数失败！"+ ex.ToString());
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var models = await repository.GetPagedListAsync(predicate:
                   x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.UserName.Contains(parameter.Search),
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
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, model);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> AddAsync(TbWeighOperatorDto tbWeighOperator)
        {
            try
            {

                var dbmodel = mapper.Map<tb_weigh_operator>(tbWeighOperator);
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.UserNumber.Equals(tbWeighOperator.UserNumber));
                if (model == null) 
                {
                    dbmodel.CreateTime = DateTime.Now;
                    dbmodel.UserCode = SystemBase.GetRndStrOnlyFor(20, true);
                    dbmodel.Password = EncryptTools.GetMD5(dbmodel.UserNumber+dbmodel.Password);
                    await unitOfWork.GetRepository<tb_weigh_operator>().InsertAsync(dbmodel);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                        return new ApiResponse(true, dbmodel);
                }
                
                return new ApiResponse(false, "添加数据失败，账号已存在");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
         
        public async Task<ApiResponse> UpdateAsync(TbWeighOperatorDto tbWeighOperator)
        {
            try
            {
                var dbmodel= mapper.Map<tb_weigh_operator>(tbWeighOperator);
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
                var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbmodel.Id));
                model.UserNumber = dbmodel.UserNumber;
                model.Password = EncryptTools.GetMD5(dbmodel.UserNumber+dbmodel.Password);
                model.UserName = dbmodel.UserName;
                model.Remark = dbmodel.Remark;
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
                var repository = unitOfWork.GetRepository<tb_weigh_operator>();
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

        public async Task<ApiResponse> MenuAuthority(string usercode)
        {
            try
            {
                var repository1 = unitOfWork.GetRepository<tb_weigh_groupauthority>();
                var repository2 = unitOfWork.GetRepository<tb_weigh_groupauthorityusers>();
                var repository3 = unitOfWork.GetRepository<tb_weigh_menu>();
          
                // Step 1: Query data from repository1 based on usercode
                var groupAuthorities = await repository1.GetAll()
                    .Where(ga => repository2.GetAll().Any(gau => gau.UserCode == usercode && gau.UserGroupCode == ga.UserGroupCode))
                    .Select(ga => ga.AuthorityCode)
                    .ToListAsync();

                // Step 2: Join the filtered data from repository1 with repository3
                var query = from m in repository3.GetAll()
                            where groupAuthorities.Contains(m.MenuCode)
                            select new MenuBarDto
                            {
                                Id = m.Id,
                                ParentId = m.MenuNumber,
                                Icon = m.Attribute2,
                                Title = m.MenuName,
                                NameSpace = m.MenuCode
                            };
                var models = await query.Distinct().ToListAsync(); 
                return new ApiResponse(true, models); 
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        } 
    }
}
