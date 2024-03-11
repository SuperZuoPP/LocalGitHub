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
    public class TbWeighMenuController : Controller
    {
        private readonly IMenuService service;
        public TbWeighMenuController(IMenuService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await service.GetSingleAsync(id);

        [HttpGet] 
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter param) => await service.GetAllAsync(param);

        [HttpGet]
        public async Task<ApiResponse> GetAllFilterAsync([FromQuery] TbWeighMenuDtoParameter param) => await service.GetAllFilterAsync(param);

        [HttpGet]
        public async Task<ApiResponse> GetMenuSum() => await service.GetMenuSum();

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] TbWeighMenuDto model) => await service.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] TbWeighMenuDto model) => await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await service.DeleteAsync(id);
    }
}

