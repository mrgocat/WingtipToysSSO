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
    public class MongoUsersRepository : MongoRepositoryBase, IUsersRepository
    {
        private readonly IMongoCollection<User> _users;
        public MongoUsersRepository(IDatabaseSettings settings, IMapper mapper) : base(settings, mapper)
        {
            _users = _database.GetCollection<User>("Users");
        }
        public void Create(UserPoco poco)
        {
            User user = _mapper.Map<User>(poco);
            _users.InsertOne(user);
        }
        public UserPoco Find(string id)
        {
            var user = _users.Find(e => e.Id == id).SingleOrDefault();
            if (user == null) return null;
            return _mapper.Map<UserPoco>(user);
        }

        public IList<UserPoco> Read()
        {
            var list = _users.Find(e => true).ToList();
            return _mapper.Map<List<UserPoco>>(list);
        }
        public void Update(UserPoco poco)
        {
            var user = _users.Find(e => e.Id == poco.Id).SingleOrDefault();
            if (user == null) return;

            user.LastName = poco.LastName;
            user.FirstName = poco.FirstName;
            user.Email = poco.Email;
            user.Nationality = poco.Nationality;
            user.Telephone = poco.Telephone;
            user.DateOfBirth = poco.DateOfBirth;
            user.LastUpdated = DateTime.Now;

            _users.ReplaceOne(e => e.Id == user.Id, user);
        }

        public void LockUser(string id)
        {
            var user = _users.Find(e => e.Id == id).SingleOrDefault();
            if (user == null) return;
            user.IsLocked = true;
            user.LastUpdated = DateTime.Now;

            _users.ReplaceOne(e => e.Id == user.Id, user);
        }

        public void UpdatePassword(string id, string passwordHash)
        {
            var user = _users.Find(e => e.Id == id).SingleOrDefault();
            if (user == null) return;
            user.PasswordHash = passwordHash;
            user.LastUpdated = DateTime.Now;

            _users.ReplaceOne(e => e.Id == user.Id, user);
        }
    }
}
