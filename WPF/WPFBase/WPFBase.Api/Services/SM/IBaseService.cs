using System.Threading.Tasks;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Services.SM
{
    public interface IBaseService<T>
    {
        Task<ApiResponse> GetAllAsync(QueryParameter query);

        Task<ApiResponse> GetSingleAsync(int id);

        Task<ApiResponse> AddAsync(T model);

        Task<ApiResponse> UpdateAsync(T model);

        Task<ApiResponse> DeleteAsync(int id);
    }
}
