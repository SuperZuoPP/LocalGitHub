using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WPFBase.Api.Services.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
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

    }
}
