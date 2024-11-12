using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WPFBase.Api.Services.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Entities.BM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Extensions;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TbWeighDatalineinfoController : Controller
    {
        private readonly ITbWeighDatalineinfoService service;
        public TbWeighDatalineinfoController(ITbWeighDatalineinfoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> GetWeightInfoByDay([FromQuery] TbWeighDatalineinfoDtoParameter parameter) => await service.GetWeightInfoByDay(parameter);

        [HttpGet]
        public async Task<ApiResponse> GetWeightInfo([FromQuery] QueryParameter parameter) 
        {
            var info = await service.GetWeightInfo(parameter);
            //PagedList<tb_weigh_datalineinfo> = info.Result
            //string paginationInfo = "{\"total\": 100, \"page\": 1, \"per_page\": 10}";

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(info.));
            return info;
        }

        [HttpGet]
        public async Task<ApiResponse> GetWeightInfoByDayRange([FromQuery] TbWeighDatalineinfoDtoParameter parameter) => await service.GetWeightInfoByDayRange(parameter);
    }
}
