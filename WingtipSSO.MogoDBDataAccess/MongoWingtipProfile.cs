using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.MongoDBDataAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.MongoDBDataAccess
{
    public class MongoWingtipProfile : Profile
    {
        public MongoWingtipProfile()
        {
            CreateMap<User, UserPoco>().ReverseMap();
            CreateMap<UserLoginLog, UserLoginLogPoco>().ReverseMap();
            CreateMap<UserHistory, UserHistoryPoco>().ReverseMap();
            CreateMap<UserWrongPassword, UserWrongPasswordPoco>().ReverseMap();
        }
    }
}
