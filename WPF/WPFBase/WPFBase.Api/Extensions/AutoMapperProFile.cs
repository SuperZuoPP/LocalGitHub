using AutoMapper;
using AutoMapper.Configuration;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.Model.SM;
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
            CreateMap<TbWeighOperator, TbWeighOperatorDto>().ReverseMap();
            CreateMap<TbWeighUsergroup, TbWeighUsergroupDto>().ReverseMap();
            CreateMap<TbWeighGroupauthorityuser, TbWeighGroupauthorityuserDto>().ReverseMap();
            CreateMap<TbWeighMenu, TbWeighMenuDto>().ReverseMap();
            CreateMap<TbWeighGroupauthority, TbWeighGroupauthorityDto>().ReverseMap(); 
            CreateMap<TbWeighDatalineinfo, TbWeighDatalineinfoDto>().ReverseMap();
            CreateMap<TbWeighLittleplan, TbWeighLittleplanDto>().ReverseMap();

        }
    }
} 