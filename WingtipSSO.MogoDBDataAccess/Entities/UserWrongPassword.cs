using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.MogoDBDataAccess.Entities
{
    public class UserWrongPassword
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public int FailedCount { get; set; }
    }
}
