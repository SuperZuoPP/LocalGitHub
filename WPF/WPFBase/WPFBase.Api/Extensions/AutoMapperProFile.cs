using AutoMapper;
using AutoMapper.Configuration;
using WPFBase.Api.Context.Model;
using WPFBase.Entities.BM;
using WPFBase.Entities.SM;
using WPFBase.Api.Context.UnitOfWork;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Api.Extensions
{
    public class AutoMapperProFile : Profile
    {
        public AutoMapperProFile()
        {
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Operator, OperatorDto>().ReverseMap();
            CreateMap<tb_weigh_operator, TbWeighOperatorDto>().ReverseMap();
            CreateMap<tb_weigh_usergroup, TbWeighUsergroupDto>().ReverseMap();
            CreateMap<tb_weigh_groupauthorityusers, TbWeighGroupauthorityuserDto>().ReverseMap();
            CreateMap<tb_weigh_menu, TbWeighMenuDto>().ReverseMap();
            CreateMap<tb_weigh_groupauthority, TbWeighGroupauthorityDto>().ReverseMap(); 
            CreateMap<tb_weigh_datalineinfo, TbWeighDatalineinfoDto>().ReverseMap();
            CreateMap<tb_weigh_littleplan, TbWeighLittleplanDto>().ReverseMap();
            CreateMap<tb_weigh_plan, TbWeighPlanDto>().ReverseMap();
        }
    }
} 