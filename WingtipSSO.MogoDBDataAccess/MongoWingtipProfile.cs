using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.MogoDBDataAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.MogoDBDataAccess
{
    public class MongoWingtipProfile : Profile
    {
        public MongoWingtipProfile()
        {
            CreateMap<User, UserPoco>().ReverseMap();
        }
    }
}
