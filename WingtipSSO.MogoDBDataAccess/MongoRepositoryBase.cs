using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.MongoDBDataAccess
{
    public abstract class  MongoRepositoryBase
    {
        protected readonly IMapper _mapper;
        protected readonly IMongoDatabase _database;

        public MongoRepositoryBase(IDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _mapper = mapper;
        }
    }
}
