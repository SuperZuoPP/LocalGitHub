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
    public class DataInfoService : BaseService<TbWeighDatalineinfoDto>,IDataInfoService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "TbWeighDatalineinfo";
        public DataInfoService(HttpRestClient client) : base(client, "TbWeighDatalineinfo")
        {
            this.client = client;
        } 

        public async Task<ApiResponse<PagedList<TbWeighDatalineinfoDto>>> GetWeightInfoByDay(TbWeighDatalineinfoDtoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetWeightInfoByDay?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&WeighTime={parameter.WeighTime}" +
                $"&CarNumber={parameter.CarNumber}" +
                $"&RecipientName={parameter.RecipientName}" +
                $"&SupplierName={parameter.SupplierName}" +
                $"&MaterialName={parameter.MaterialName}" +
                $"&WeighHouseCodes={parameter.WeighHouseCodes}";
            request.Parameter = parameter;
            return await client.ExecuteAsync<PagedList<TbWeighDatalineinfoDto>>(request); 
        }

        public async Task<ApiResponse<PagedList<TbWeighDatalineinfoDto>>> GetWeightInfoByDayRange(TbWeighDatalineinfoDtoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/{serviceName}/GetWeightInfoByDayRange?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&BeginWeighTime={parameter.BeginWeighTime}" +
                $"&EndWeighTime={parameter.EndWeighTime}" +
                $"&CarNumber={parameter.CarNumber}" +
                $"&RecipientName={parameter.RecipientName}" +
                $"&SupplierName={parameter.SupplierName}" +
                $"&MaterialName={parameter.MaterialName}" +
                $"&WeighHouseCodes={parameter.WeighHouseCodes}";
            request.Parameter = parameter;
            return await client.ExecuteAsync<PagedList<TbWeighDatalineinfoDto>>(request);
        }
    }
}
 