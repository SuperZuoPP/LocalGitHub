using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighUsergroupService:IBaseService<TbWeighUsergroupDto>
    { 
        Task<ApiResponse> GetUserList(TbWeighOperatorDtoParameter query);

        Task<ApiResponse> GetUserSum();
    }
}
