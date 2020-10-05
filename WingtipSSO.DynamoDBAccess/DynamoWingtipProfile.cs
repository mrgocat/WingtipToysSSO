using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WingtipSSO.DynamoDBAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.DynamoDBAccess
{
    public class DynamoWingtipProfile : Profile
    {
        public DynamoWingtipProfile()
        {
            CreateMap<User, UserPoco>()
                .ForMember(m => m.Roles, map => map.MapFrom(u => MapRole(u)));
            CreateMap<UserPoco, User>()
                .ForMember(m => m.Roles, map => map.MapFrom(u => MapRole(u)));
            CreateMap<UserLoginLog, UserLoginLogPoco>().ReverseMap();
            CreateMap<UserHistory, UserHistoryPoco>().ReverseMap();
            CreateMap<UserWrongPassword, UserWrongPasswordPoco>().ReverseMap();
        }
        private string MapRole(UserPoco poco)
        {
            if (poco.Roles != null && poco.Roles.Count > 0)
            {
                string[] roles = poco.Roles.Select(role => role.ToString()).ToArray();
                return string.Join(",", roles);
            }
            return null;
        }
        private IList<Role> MapRole(User entity)
        {
            if(entity.Roles == null) return null;
            
            IList<Role> list = entity.Roles.Split(",").Select(s => (Role)Enum.Parse(typeof(Role), s)).ToList();
            return list;
        }
    }
}
