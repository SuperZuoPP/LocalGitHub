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
    public class TbWeighWeighbridgeofficeController : Controller
    {
        private readonly ITbWeighWeighbridgeofficeService service;
        public TbWeighWeighbridgeofficeController(ITbWeighWeighbridgeofficeService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> GetList() => await service.GetList();

    }
}
