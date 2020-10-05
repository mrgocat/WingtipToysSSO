using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.MongoDBDataAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.MongoDBDataAccess
{
    public class MongoUserLoginLogsRepository : MongoRepositoryBase, IUserLoginLogsRepository
    {
        private readonly IMongoCollection<UserLoginLog> _userLoginLogs;
        public MongoUserLoginLogsRepository(IDatabaseSettings settings, IMapper mapper) : base(settings, mapper)
        {
            _userLoginLogs = _database.GetCollection<UserLoginLog>("UserLoginLogs");
        }
        public string Create(UserLoginLogPoco poco)
        {
            UserLoginLog entity = _mapper.Map<UserLoginLog>(poco);
            _userLoginLogs.InsertOne(entity);
            return entity.Id;
        }
    }
}
