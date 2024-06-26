﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Services.ServiceBase;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Extensions;
using WPFBase.Shared.Parameters;

namespace WPFBase.Services
{
    public class LoginService : BaseService<TbWeighOperatorDto> ,ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "LoginWeigh";

        public LoginService(HttpRestClient client) : base(client, "LoginWeigh")
        {
            this.client = client;
        }

        public async Task<ApiResponse<TbWeighOperatorDto>> Login(TbWeighOperatorDto tbWeighOperatorDto)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Post;
            baseRequest.Route = $"api/{serviceName}/Login";
            baseRequest.Parameter = tbWeighOperatorDto;
            return await client.ExecuteAsync<TbWeighOperatorDto>(baseRequest);
        }

        public async Task<ApiResponse> Resgiter(TbWeighOperatorDto tbWeighOperatorDto)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Post;
            baseRequest.Route = $"api/{serviceName}/Resgiter";
            baseRequest.Parameter = tbWeighOperatorDto;
            return await client.ExecuteAsync(baseRequest);
        }

        public async Task<ApiResponse<PagedList<TbWeighOperatorDto>>> GetAllFilterAsync(TbWeighOperatorDtoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetAllFilter?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}" +
                $"&status={parameter.Status}";
            return await client.ExecuteAsync<PagedList<TbWeighOperatorDto>>(request);
        }

        public async Task<ApiResponse> Summary( )
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Get;
            baseRequest.Route = $"api/{serviceName}/Summary";
            baseRequest.Parameter = "";
            return await client.ExecuteAsync(baseRequest);
        }

        public async Task<ApiResponse> MenuAuthority(string usercode)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Get;
            baseRequest.Route = $"api/{serviceName}/MenuAuthority?usercode={usercode}";
            baseRequest.Parameter = usercode;
            return await client.ExecuteAsync(baseRequest);
        }
    }
}
