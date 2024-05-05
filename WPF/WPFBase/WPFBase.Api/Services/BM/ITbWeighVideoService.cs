using System.Threading.Tasks;
using WPFBase.Api.Services.SM; 
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighVideoService : IBaseService<TbWeighVideoDto>
    { 
        Task<ApiResponse> GetVideoList(TbWeighVideoDtoParameter parameter);
         
    }
}
