using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Services.ServiceBase;
using WPFBase.Shared;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Extensions;
using WPFBase.Shared.Parameters;

namespace WPFBase.Services
{
    public class MenuService : BaseService<TbWeighMenuDto>,IMenuService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "TbWeighMenu";
        public MenuService(HttpRestClient client) : base(client, "TbWeighMenu")
        {
            this.client = client;
        }
         

        public async Task<ApiResponse> GetMenuSum()
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Get;
            baseRequest.Route = $"api/{serviceName}/GetMenuSum";
            baseRequest.Parameter = "";
            return await client.ExecuteAsync(baseRequest);
        }

        public async Task<ApiResponse<PagedList<TbWeighMenuDto>>> GetAllFilterAsync(TbWeighMenuDtoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetAllFilter?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}" +
                $"&status={parameter.Status}";
            return await client.ExecuteAsync<PagedList<TbWeighMenuDto>>(request);
        }
         
    }
}
