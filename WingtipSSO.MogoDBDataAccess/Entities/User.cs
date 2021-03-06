﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.MongoDBDataAccess.Entities
{
    public class User
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public bool ForceChangePassword { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActivated { get; set; }
        public bool IsLocked { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual IList<string> Roles { get; set; }
    }
}
