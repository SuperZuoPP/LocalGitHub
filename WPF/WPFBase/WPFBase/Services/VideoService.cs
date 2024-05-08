using System;
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
    public class VideoService : BaseService<TbWeighVideoDto> ,IVideoService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "TbWeighVideo";

        public VideoService(HttpRestClient client) : base(client, "TbWeighVideo")
        {
            this.client = client;
        }
          
      

        public async Task<ApiResponse> GetVideoList(TbWeighVideoDtoParameter parameter)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Get;
            baseRequest.Route = $"api/{serviceName}/GetVideoList?VideoTypeNo={parameter.VideoTypeNo}" +
                $"&Status={parameter.Status}";
            baseRequest.Parameter = parameter;
            return await client.ExecuteAsync(baseRequest); 
        }


        public async Task<ApiResponse> GetDvrMonitorChannelList(TbWeighVideoDtoParameter parameter)
        {
            BaseRequest baseRequest = new BaseRequest();
            baseRequest.Method = RestSharp.Method.Get;
            baseRequest.Route = $"api/{serviceName}/GetDvrMonitorChannelList?DeviceNo={parameter.DeviceNo}" +
                $"&Status={parameter.Status}" +
                $"&WeighHouseCodes={parameter.WeighHouseCodes}";
            baseRequest.Parameter = parameter;
            return await client.ExecuteAsync(baseRequest);

        }
    }
}
