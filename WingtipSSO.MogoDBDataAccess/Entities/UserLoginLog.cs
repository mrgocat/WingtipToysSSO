using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.MogoDBDataAccess.Entities
{
    public class UserLoginLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginDate { get; set; }
        public bool Failed { get; set; }
        public string FailedReason { get; set; }
    }
}
