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
    public class MongoUsersRepository : IUsersRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMapper _mapper;
        public MongoUsersRepository(IDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("Users");
            _mapper = mapper;
        }
        public UserPoco Create(UserPoco poco)
        {
            User user = _mapper.Map<User>(poco);
            _users.InsertOne(user);
            return poco;
        }

        public UserPoco Find(string id)
        {
            var user = _users.Find(sub => sub.Id == id).SingleOrDefault();
            if (user == null) return null;
            return _mapper.Map<UserPoco>(user);
        }

        public IList<UserPoco> Read()
        {
            var list = _users.Find(sub => true).ToList();
            return _mapper.Map<List<UserPoco>>(list);
        }
    }
}
