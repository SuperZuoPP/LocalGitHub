using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;
using WPFBase.Shared.Extensions;

namespace WPFBase.Services
{
    public interface ILoginService
    {
        Task<ApiResponse<TbWeighOperatorDto>> Login(TbWeighOperatorDto tbWeighOperatorDto);

        Task<ApiResponse> Resgiter(TbWeighOperatorDto tbWeighOperatorDto);

        Task<ApiResponse<PagedList<TbWeighOperatorDto>>> GetAllFilterAsync(TbWeighOperatorDtoParameter parameter);
    }
}
