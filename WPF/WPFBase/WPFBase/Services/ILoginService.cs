using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.Services
{
    public interface ILoginService
    {
        Task<ApiResponse<TbWeighOperatorDto>> Login(TbWeighOperatorDto tbWeighOperatorDto);

        Task<ApiResponse> Resgiter(TbWeighOperatorDto tbWeighOperatorDto);

    }
}
