using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.DynamoDBAccess.Entities
{
    [DynamoDBTable("UserHistories")]
    public class UserHistory
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Updated { get; set; }
        public User SnapShot { get; set; }
        public string UpdateUserId { get; set; }
        public string UpdateReason { get; set; }
    }
}
