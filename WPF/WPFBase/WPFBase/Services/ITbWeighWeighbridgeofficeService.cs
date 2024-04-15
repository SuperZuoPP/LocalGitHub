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
    public interface ITbWeighWeighbridgeofficeService : IBaseService<TbWeighWeighbridgeofficeDTO>
    {
        Task<ApiResponse<PagedList<TbWeighWeighbridgeofficeDTO>>> GetList();  
    }
}
