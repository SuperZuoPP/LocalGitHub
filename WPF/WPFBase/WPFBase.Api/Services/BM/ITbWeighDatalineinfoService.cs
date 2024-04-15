using System.Threading.Tasks;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighDatalineinfoService : IBaseService<TbWeighDatalineinfoDto>
    { 
        Task<ApiResponse> GetWeightInfoByDay(TbWeighDatalineinfoDtoParameter parameter);

        Task<ApiResponse> GetWeightInfoByDayRange(TbWeighDatalineinfoDtoParameter parameter);
    }
}
