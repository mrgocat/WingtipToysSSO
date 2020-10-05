using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.DynamoDBAccess.Entities
{
    public class UserWrongPassword
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public int FailedCount { get; set; }
    }
}
