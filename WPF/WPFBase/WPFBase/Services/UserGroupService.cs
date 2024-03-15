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

        public async Task<ApiResponse<PagedList<TbWeighGroupauthorityuserDto>>> GetUserGroupAndUserList(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/GetUserGroupAndUserList?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}" ;
            return await client.ExecuteAsync<PagedList<TbWeighGroupauthorityuserDto>>(request);
        }

        public async Task<ApiResponse> GroupUserAdd(TbWeighGroupauthorityuserDto parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/GroupUserAdd";
            request.Parameter = parameter;
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse> GroupUserRemove(TbWeighGroupauthorityuserDto parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/GroupUserRemove";
            request.Parameter = parameter;
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<PagedList<TbWeighGroupauthorityDto>>> GetGroupAuthority(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{serviceName}/GetGroupAuthority?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}";
            return await client.ExecuteAsync<PagedList<TbWeighGroupauthorityDto>>(request);
        }

        public async Task<ApiResponse> GroupAuthorityAdd(TbWeighGroupauthorityDto parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/GroupAuthorityAdd";
            request.Parameter = parameter;
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse> GroupAuthorityRemove(TbWeighGroupauthorityDto parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/GroupAuthorityRemove";
            request.Parameter = parameter;
            return await client.ExecuteAsync(request);
        }
    }
}
