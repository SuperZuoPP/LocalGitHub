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
    public class UserGroupService : BaseService<TbWeighUsergroupDto>,IUserGroupService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "TbWeighUsergroup";
        public UserGroupService(HttpRestClient client) : base(client, "TbWeighUsergroup")
        {
            this.client = client;
        }
         
        public async Task<ApiResponse<PagedList<TbWeighOperatorDto>>> GetUserList(TbWeighOperatorDtoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/GetUserList?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}" +
                $"&status={parameter.Status}";
            return await client.ExecuteAsync<PagedList<TbWeighOperatorDto>>(request);
        }

        public async Task<ApiResponse> GetUserSum()
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.GET;
            baseRequest.Route = $"api/{serviceName}/GetUserSum";
            baseRequest.Parameter = "";
            return await client.ExecuteAsync(baseRequest);
        }
    }
}
