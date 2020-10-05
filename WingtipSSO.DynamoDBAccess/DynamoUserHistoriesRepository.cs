using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.DynamoDBAccess.Entities;
using WingtipSSO.POCOS;

namespace WingtipSSO.DynamoDBAccess
{
    public class DynamoUserHistoriesRepository : DynamoRepositoryBase, IUserHistoriesRepository
    {
        public DynamoUserHistoriesRepository(DynamoDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public string Create(string userId, string updateUserId, string updateReason)
        {
            var user = _context.LoadAsync<User>(userId, new DynamoDBContextConfig
            {
                ConsistentRead = true
            }).Result;
           
            UserHistory entity = new UserHistory
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Updated = user.LastUpdated,
                SnapShot = user,
                UpdateUserId = updateUserId,
                UpdateReason = updateReason
            };

            _context.SaveAsync(entity);

            return entity.Id;
        }
    }
}
