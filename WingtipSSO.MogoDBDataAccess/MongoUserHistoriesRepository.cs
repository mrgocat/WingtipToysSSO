using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.MogoDBDataAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.MogoDBDataAccess
{
    public class MongoUserHistoriesRepository : MongoRepositoryBase, IUserHistoriesRepository
    {
        private readonly IMongoCollection<UserHistory> _userHistories;
        public MongoUserHistoriesRepository(IDatabaseSettings settings, IMapper mapper) : base(settings, mapper)
        {
            _userHistories = _database.GetCollection<UserHistory>("UserHistories");
        }
        /*public string Create(UserHistoryPoco poco)
        {
            UserHistory entity = _mapper.Map<UserHistory>(poco);
            _userHistories.InsertOne(entity);
            return entity.Id;
        }*/

        public string Create(string userId, string updateUserId, string updateReason)
        {
            var users = _database.GetCollection<User>("Users");
            User user = users.Find(e => e.Id == userId).FirstOrDefault();
            UserHistory uhPoco = new UserHistory
            {
                UserId = userId,
                Updated = user.LastUpdated,
                SnapShot = user,
                UpdateUserId = updateUserId,
                UpdateReason = updateReason
            };
            _userHistories.InsertOne(uhPoco);
            return uhPoco.Id;
        }
    }
}
