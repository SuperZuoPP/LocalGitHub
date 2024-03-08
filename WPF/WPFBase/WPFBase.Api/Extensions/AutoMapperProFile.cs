using AutoMapper.Configuration;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.Model.SM;
using WPFBase.Shared.DTO.BM;
using WPFBase.Shared.DTO.SM;

namespace WPFBase.Api.Extensions
{
    public class AutoMapperProFile : MapperConfigurationExpression
    {
        public AutoMapperProFile()
        {
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Operator, OperatorDto>().ReverseMap();
            CreateMap<TbWeighOperator, TbWeighOperatorDto>().ReverseMap();
            CreateMap<TbWeighUsergroup, TbWeighUsergroupDto>().ReverseMap();
        }
    }
}
