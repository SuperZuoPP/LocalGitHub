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


        //获取启用的类型设备列表
        [HttpGet]
        public async Task<ApiResponse> GetVideoList([FromQuery] TbWeighVideoDtoParameter parameter) => await service.GetVideoList(parameter);


        //获取指定磅房，指定硬盘录像机，指定摄像头类型，指定启用状态的通道对应的摄像机列表
        [HttpGet]
        public async Task<ApiResponse> GetDvrMonitorChannelList([FromQuery] TbWeighVideoDtoParameter parameter) => await service.GetDvrMonitorChannelList(parameter);

    }
}
