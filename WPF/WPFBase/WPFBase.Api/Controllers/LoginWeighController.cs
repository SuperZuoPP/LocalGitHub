using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WPFBase.Api.Services.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginWeighController : Controller
    {
        private readonly ITbWeighOperatorService service;
        public LoginWeighController(ITbWeighOperatorService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] TbWeighOperatorDto param) => await service.LoginAsync(param.UserNumber, param.PassWord);

        [HttpPost]
        //[Authorize]
        public async Task<ApiResponse> Resgiter([FromBody] TbWeighOperatorDto param) => await service.Resgiter(param);

        [HttpGet]
        public async Task<ApiResponse> GetAllFilterAsync([FromQuery] TbWeighOperatorDtoParameter param) => await service.GetAllFilterAsync(param);

        [HttpGet]
        public async Task<ApiResponse> Summary() => await service.Summary(); 
    }
}
