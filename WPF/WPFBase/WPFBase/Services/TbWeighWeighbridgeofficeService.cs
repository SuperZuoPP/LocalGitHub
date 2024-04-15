using DryIoc;
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
    public class TbWeighWeighbridgeofficeService : BaseService<TbWeighWeighbridgeofficeDTO>, ITbWeighWeighbridgeofficeService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "TbWeighWeighbridgeoffice";
        public TbWeighWeighbridgeofficeService(HttpRestClient client) : base(client, "TbWeighWeighbridgeoffice")
        {
            this.client = client;
        }

        public async Task<ApiResponse<PagedList<TbWeighWeighbridgeofficeDTO>>> GetList()
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Get;
            baseRequest.Route = $"api/{serviceName}/GetList";
            baseRequest.Parameter = "";
            return await client.ExecuteAsync<PagedList<TbWeighWeighbridgeofficeDTO>>(baseRequest);
             
        }
    }
}
