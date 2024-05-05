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
    public class TbWeighVideoController : Controller
    {
        private readonly ITbWeighVideoService service;
        public TbWeighVideoController(ITbWeighVideoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> GetVideoList([FromQuery] TbWeighVideoDtoParameter parameter) => await service.GetVideoList(parameter);
 
    }
}
