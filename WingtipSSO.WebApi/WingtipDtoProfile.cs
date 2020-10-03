using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingtipSSO.POCOS;
using WingtipSSO.WebApi.Models;

namespace WingtipSSO.WebApi
{
    public class WingtipDtoProfile : Profile
    {
        public WingtipDtoProfile()
        {
            CreateMap<UserPoco, UserDto>();
            CreateMap<UserCreateDto, UserPoco>();
            CreateMap<UserUpdateDto, UserPoco>();
        }
    }
}
