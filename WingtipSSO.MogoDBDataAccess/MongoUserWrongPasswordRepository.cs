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
    public class MongoUserWrongPasswordRepository : MongoRepositoryBase, IUserWrongPasswordRepository
    {
        private readonly IMongoCollection<UserWrongPassword> _userWrongPassword;
        public MongoUserWrongPasswordRepository(IDatabaseSettings settings, IMapper mapper) : base(settings, mapper)
        {
            _userWrongPassword = _database.GetCollection<UserWrongPassword>("UserWrongPassword");
        }
        public string Create(UserWrongPasswordPoco poco)
        {
            UserWrongPassword entity = _mapper.Map<UserWrongPassword>(poco);
            _userWrongPassword.InsertOne(entity);
            return entity.Id;
        }
        public void IncreaseFailedCount(string userId)
        {
            UserWrongPassword entity = _userWrongPassword.Find(e => e.Id == userId).SingleOrDefault();
            if (entity == null)
            {
                entity = new UserWrongPassword
                {
                    Id = userId,
                    FailedCount = 1
                };
                _userWrongPassword.InsertOne(entity);
            }
            else
            {
                entity.FailedCount++;
                _userWrongPassword.ReplaceOne(e => e.Id == entity.Id, entity);
            }
        }
        public void Delete(string userId)
        {
            _userWrongPassword.DeleteOne(e => e.Id == userId);
        }

        public UserWrongPasswordPoco Find(string userId)
        {
            UserWrongPassword entity = _userWrongPassword.Find(e => e.Id == userId).SingleOrDefault();
            if (entity == null) return null;
            return _mapper.Map<UserWrongPasswordPoco>(entity);
        }
    }
}
