using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WPFBase.Api.Services.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly ILoginService service;
        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] OperatorDto param) => await service.LoginAsync(param.UserNumber, param.PassWord);

        [HttpPost]
        //[Authorize]
        public async Task<ApiResponse> Resgiter([FromBody] OperatorDto param) => await service.Resgiter(param);


    }
}
