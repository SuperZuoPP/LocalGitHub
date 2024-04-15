using System.Threading.Tasks;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighWeighbridgeofficeService : IBaseService<TbWeighWeighbridgeofficeDTO>
    { 
        Task<ApiResponse> GetList(); 
    }
}
