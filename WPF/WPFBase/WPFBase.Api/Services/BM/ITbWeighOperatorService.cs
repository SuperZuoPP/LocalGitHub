using System.Threading.Tasks;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighOperatorService
    {
        Task<ApiResponse> LoginAsync(string account, string password);

        Task<ApiResponse> Resgiter(TbWeighOperatorDto tbWeighOperatorDto);
    }
}

