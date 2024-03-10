using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Services.BM;
using WPFBase.Api.Services.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.Parameters;

namespace WPFBase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TbWeighUsergroupController:Controller
    {
        private readonly ITbWeighUsergroupService service;

        public TbWeighUsergroupController(ITbWeighUsergroupService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await service.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter parameter) => await service.GetAllAsync(parameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] TbWeighUsergroupDto model) => await service.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] TbWeighUsergroupDto model) => await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await service.DeleteAsync(id);

        
       [HttpGet]
        public async Task<ApiResponse> GetUserList([FromQuery] TbWeighOperatorDtoParameter parameter) => await service.GetUserList(parameter);

        [HttpGet]
        public async Task<ApiResponse> GetUserSum() => await service.GetUserSum();

        [HttpGet]
        public async Task<ApiResponse> GetUserGroupAndUserList([FromQuery] QueryParameter parameter) => await service.GetUserGroupAndUserList(parameter);

        [HttpPost]
        public async Task<ApiResponse> GroupUserAdd([FromBody] TbWeighGroupauthorityuserDto parameter) => await service.GroupUserAdd(parameter);


        [HttpPost]
        public async Task<ApiResponse> GroupUserRemove([FromBody] TbWeighGroupauthorityuserDto parameter) => await service.GroupUserRemove(parameter);
    }
}
