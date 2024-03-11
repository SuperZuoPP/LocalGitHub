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
using WPFBase.Services.ServiceBase;

namespace WPFBase.Services
{
    public interface IMenuService : IBaseService<TbWeighMenuDto>
    {
       
        Task<ApiResponse<PagedList<TbWeighMenuDto>>> GetAllFilterAsync(TbWeighMenuDtoParameter parameter);

        Task<ApiResponse> GetMenuSum();
    }
}
