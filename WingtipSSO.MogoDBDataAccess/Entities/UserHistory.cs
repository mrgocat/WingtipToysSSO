using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.MogoDBDataAccess.Entities
{
    public class UserHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Updated { get; set; }
        public User SnapShot { get; set; }
        public string UpdateUserId { get; set; }
        public string UpdateReason { get; set; }
    }
}
