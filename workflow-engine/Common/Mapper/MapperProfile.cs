using AutoMapper;
using Common.Dto;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<FlowExecutions, FlowExecutionsDto>().ForMember(m => m.User, c=>c.MapFrom(s => s.User));
            CreateMap<User, UserDto>();
        }
    }
}
