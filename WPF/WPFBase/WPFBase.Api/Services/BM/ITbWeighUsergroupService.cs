﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighUsergroupService:IBaseService<TbWeighUsergroupDto>
    { 
        Task<ApiResponse> GetUserList(TbWeighOperatorDtoParameter query);

        Task<ApiResponse> GetUserSum();

        Task<ApiResponse> GetUserGroupAndUserList(QueryParameter query);

        Task<ApiResponse> GroupUserAdd(TbWeighGroupauthorityuserDto parameter);

        Task<ApiResponse> GroupUserRemove(TbWeighGroupauthorityuserDto parameter);

        Task<ApiResponse> GetGroupAuthority(QueryParameter parameter);

        Task<ApiResponse> GroupAuthorityAdd(TbWeighGroupauthorityDto parameter);

        Task<ApiResponse> GroupAuthorityRemove(TbWeighGroupauthorityDto parameter);
    }
}
