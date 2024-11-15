﻿using AutoMapper;
using System;
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

namespace WPFBase.Api.Services.BM
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try
            {
                password = EncryptTools.GetMD5(account+password);
                var model = await  unitOfWork.GetRepository<Operator>().GetFirstOrDefaultAsync(predicate: x => (x.UserNumber.Equals(account)) && (x.Password.Equals(password)));
                if (model == null)
                    return new ApiResponse("账号密码错误！请重新输入！");

                return new ApiResponse(true, new OperatorDto()
                {
                   
                    UserNumber = model.UserNumber,
                    UserName = model.UserName,
                    Id = model.Id
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "登录失败！"+ex.ToString());
            }
        }

        public async Task<ApiResponse> Resgiter(OperatorDto operatorDto)
        {
            try
            {
                var model = mapper.Map<Operator>(operatorDto);
                var repository = unitOfWork.GetRepository<Operator>();
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
         
    }
}
