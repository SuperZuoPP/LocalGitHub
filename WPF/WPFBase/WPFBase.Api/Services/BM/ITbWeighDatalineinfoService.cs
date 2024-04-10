using System.Threading.Tasks;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighDatalineinfoService : IBaseService<TbWeighDatalineinfoDto>
    {
        //查询流水 小计划单和明细表  
        Task<ApiResponse> GetWeightInfoByDay(TbWeighDatalineinfoDtoParameter parameter); 
    }
}
