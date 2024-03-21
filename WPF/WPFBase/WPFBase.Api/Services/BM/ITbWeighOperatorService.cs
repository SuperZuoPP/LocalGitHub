using System.Threading.Tasks;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared.Extensions;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.BM
{
    public interface ITbWeighOperatorService:IBaseService<TbWeighOperatorDto>
    {
        Task<ApiResponse> LoginAsync(string account, string password);

        Task<ApiResponse> Resgiter(TbWeighOperatorDto tbWeighOperatorDto);

        Task<ApiResponse> GetAllFilterAsync(TbWeighOperatorDtoParameter patameter);

        Task<ApiResponse> Summary();

        Task<ApiResponse> MenuAuthority(string usercode);
    }
}

